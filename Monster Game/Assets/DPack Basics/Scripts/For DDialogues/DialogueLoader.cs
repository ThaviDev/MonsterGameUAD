using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class DialogueLoader : MonoBehaviour
{
    // ESTE CODIGO NO TOMA EN CUENTA CALLBACKS NI "IsSkippable", ES UN SISTEMA MÁS SENCILLO
    public DialogManager DialogManager;
    [SerializeField] private string initialDialogueKey = "start";
    [SerializeField] private string dialogueFilePath = "Dialogos/dialogos";

    private Dictionary<string, DialogueLine> dialogueDictionary;

    private class DialogueLine
    {
        public string Character;
        public string NextKey;
        public string Text;
    }

    void Awake()
    {
        LoadDialogues();
        ShowInitialDialogue();
    }

    private void LoadDialogues()
    {
        dialogueDictionary = new Dictionary<string, DialogueLine>();

        TextAsset file = Resources.Load<TextAsset>(dialogueFilePath);
        if (file == null)
        {
            Debug.LogError($"Archivo de diálogos no encontrado: {dialogueFilePath}");
            return;
        }

        string[] lines = file.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] parts = line.Split('\t');
            if (parts.Length >= 4)
            {
                string key = parts[0].Trim();
                string character = parts[1].Trim();
                string nextKey = parts[2].Trim();
                string text = parts[3].Trim();

                dialogueDictionary[key] = new DialogueLine
                {
                    Character = character,
                    NextKey = nextKey,
                    Text = text
                };
            }
        }
    }

    public void ShowInitialDialogue()
    {
        if (!string.IsNullOrEmpty(initialDialogueKey))
        {
            ShowDialogueChain(initialDialogueKey);
        }
    }

    public void ShowDialogueChain(string startKey)
    {
        List<DialogData> dialogDataList = BuildDialogueChain(startKey);

        if (dialogDataList != null && dialogDataList.Count > 0)
        {
            DialogManager.Show(dialogDataList);
        }
        else
        {
            Debug.LogWarning($"No se encontró diálogo para la clave: {startKey}");
        }
    }

    private List<DialogData> BuildDialogueChain(string startKey)
    {
        List<DialogData> dialogDataList = new List<DialogData>();
        string currentKey = startKey;
        HashSet<string> processedKeys = new HashSet<string>();

        while (!string.IsNullOrEmpty(currentKey) && !processedKeys.Contains(currentKey))
        {
            processedKeys.Add(currentKey);

            if (dialogueDictionary.TryGetValue(currentKey, out DialogueLine line))
            {
                // Crear DialogData manteniendo todos los comandos especiales de D'Dialogue
                DialogData dialogData = new DialogData(line.Text, line.Character);
                dialogDataList.Add(dialogData);

                // Pasar a la siguiente clave
                currentKey = line.NextKey;
            }
            else
            {
                Debug.LogWarning($"Clave de diálogo no encontrada: {currentKey}");
                break;
            }
        }

        return dialogDataList;
    }
}
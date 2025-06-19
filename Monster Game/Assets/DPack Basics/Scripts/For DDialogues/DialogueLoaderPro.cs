using Doublsb.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogueLoaderPro : MonoBehaviour
{
    public DialogManager DialogManager;
    [SerializeField] private string initialDialogueKey = "start";
    [SerializeField] private string dialogueFilePath = "Dialogos/dialogos";

    private Dictionary<string, DialogueLine> dialogueDictionary;
    private Dictionary<string, UnityAction> callbackRegistry = new Dictionary<string, UnityAction>(); // Cambiado a UnityAction

    private class DialogueLine
    {
        public string Key;
        public string Character;
        public string NextKey;
        public string Text;
        public string CallbackName;
        public bool IsSkipable = true;
    }

    void Awake()
    {
        // Registrar todos los callbacks posibles
        RegisterCallbacks();

        LoadDialogues();
    }

    private void RegisterCallbacks()
    {
        RegisterCallback("SHOW_IMAGE_1", () => Debug.Log("Mostrando imagen 1"));
        RegisterCallback("PLAY_SOUND", () => Debug.Log("Reproduciendo sonido"));
        RegisterCallback("OPEN_DOOR", () => Debug.Log("Abriendo puerta"));

        // Ejemplo con parámetro (usaremos una convención especial)
        RegisterCallback("SHOW_ITEM:1", () => ShowSpecificItem(1));
        RegisterCallback("SHOW_ITEM:2", () => ShowSpecificItem(2));
    }

    public void RegisterCallback(string callbackName, UnityAction action) // Cambiado a UnityAction
    {
        if (callbackRegistry.ContainsKey(callbackName))
        {
            Debug.LogWarning($"Callback '{callbackName}' ya está registrado. Sobrescribiendo.");
            callbackRegistry[callbackName] = action;
        }
        else
        {
            callbackRegistry.Add(callbackName, action);
        }
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
                DialogueLine dl = new DialogueLine
                {
                    Key = parts[0].Trim(),
                    Character = parts[1].Trim(),
                    NextKey = parts[2].Trim(),
                    Text = parts[3].Trim(),
                    CallbackName = (parts.Length > 4) ? parts[4].Trim() : null,
                    IsSkipable = (parts.Length > 5) ? bool.Parse(parts[5].Trim()) : true
                };
                dialogueDictionary[dl.Key] = dl;
            }
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
                // Obtener el callback como UnityAction
                UnityAction callback = null;
                if (!string.IsNullOrEmpty(line.CallbackName))
                {
                    if (callbackRegistry.TryGetValue(line.CallbackName, out UnityAction registeredCallback))
                    {
                        callback = registeredCallback;
                    }
                    else
                    {
                        Debug.LogWarning($"Callback '{line.CallbackName}' no registrado para la clave: {line.Key}");
                    }
                }

                // Crear DialogData con el callback correcto
                DialogData dialogData = new DialogData(
                    line.Text,
                    line.Character,
                    callback, // Ahora es UnityAction
                    line.IsSkipable
                );

                dialogDataList.Add(dialogData);
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

    private void InitializeCallbacks()
    {
        // Registrar callbacks con UnityAction
        RegisterCallback("SHOW_ITEM_1", new UnityAction(() => ShowSpecificItem(1)));
        RegisterCallback("SHOW_ITEM_2", new UnityAction(() => ShowSpecificItem(2)));
        RegisterCallback("START_BATTLE", new UnityAction(StartBattleSequence));
    }

    // ===== EJEMPLOS DE CALLBACKS =====
    private void ShowSpecificItem(int itemId)
    {
        Debug.Log($"Mostrando item específico: {itemId}");
    }

    private void StartBattleSequence()
    {
        Debug.Log("Iniciando secuencia de batalla!");
    }
}
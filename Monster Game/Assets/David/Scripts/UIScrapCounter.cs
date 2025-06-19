using TMPro;
using UnityEngine;

public class UIScrapCounter : MonoBehaviour
{
    [SerializeField] TMP_Text _scrapTxt;
    [SerializeField] IntSCOB _SCOB_currentScrapValue;

    void Update()
    {
        _scrapTxt.text = "Scrap: " + _SCOB_currentScrapValue.SCOB_Value.ToString();
    }
}

using TMPro;
using UnityEngine;

public class UICurrencyCounter : MonoBehaviour
{
    [SerializeField] TMP_Text _scrapTxt;
    [SerializeField] IntSCOB _SCOB_currentScrapValue;

    void Update()
    {
        _scrapTxt.text = "Coins: " + _SCOB_currentScrapValue.SCOB_Value.ToString();
    }
}

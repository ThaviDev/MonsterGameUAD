using TMPro;
using UnityEngine;

public class TradeFunction : MonoBehaviour
{
    [SerializeField] DoorFinalFunction _finalDoor;
    [SerializeField] GameObject _tradeUI;
    [SerializeField] IntSCOB _SCOB_scrapAmount;
    [SerializeField] TMP_Text _tradeText;
    [SerializeField] int _scrapAmount;

    private bool _canAccessTrade = true;

    void Start()
    {
        _canAccessTrade = true;
        _tradeText.text = "I can open the door if you give me " + _scrapAmount.ToString() + " Coins";
    }

    void Update()
    {
        
    }

    public void BTN_AcceptTrade()
    {
        if (_SCOB_scrapAmount.SCOB_Value >= _scrapAmount)
        {
            _SCOB_scrapAmount.SCOB_Value -= _scrapAmount;
            _canAccessTrade = false;
            OpenDoor();
            _tradeUI.SetActive(false);
        }
    }

    void OpenDoor()
    {
        _finalDoor.OpenDoor();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _canAccessTrade)
        {
            _tradeUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _canAccessTrade)
        {
            _tradeUI.SetActive(false);
        }
    }
}

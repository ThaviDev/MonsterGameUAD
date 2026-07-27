using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] FloatSCOB _pyrHealth;
    [SerializeField] FloatSCOB _pyrStamina;

    [SerializeField] TMP_Text _timeTxt;
    [SerializeField] TMP_Text _endTime;
    [SerializeField] Slider _healthSliderLeft;
    [SerializeField] Slider _healthSliderRight;
    [SerializeField] Slider _staminaSliderLeft;
    [SerializeField] Slider _staminaSliderRight;
    [SerializeField] TMP_Text _staminaText;
    [SerializeField] TMP_Text _healthText;
    [SerializeField] GameObject _gameOvrPanel;
    [SerializeField] GameObject _gameplayPanel;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] TMP_Text _pagesCollectedText;

    float _matchTime;
    [SerializeField] int _secondsCount;
    [SerializeField] int _minutesCount;
    bool _canCount;

    private void Awake()
    {
        GMTestGameplay.OnGameOver += GameOver;
        GMTestGameplay.OnGameOver += StopCounting;
        GMTestGameplay.OnVictory += SetVictoryPanel;
        GMTestGameplay.OnPageCollected += UpdatePagesCollected;
    }
    void Start()
    {
        _canCount = true;
        //PlayerStadistics.OnPyrDeath += StopCounting;
    }
    void Update()
    {
        //_healthSlider.value = _pyrHealth.SCOB_Value;
        _healthSliderLeft.value = _pyrHealth.SCOB_Value;
        _healthSliderRight.value = _pyrHealth.SCOB_Value;
        _staminaSliderLeft.value = _pyrStamina.SCOB_Value;
        _staminaSliderRight.value = _pyrStamina.SCOB_Value;
        var intergerHealth = (int)_pyrHealth.SCOB_Value;
        var intergerStamina = (int)_pyrStamina.SCOB_Value;
        _healthText.text = intergerHealth.ToString();
        _staminaText.text = intergerStamina.ToString();
        

        //CountTime();
    }
    void StopCounting()
    {
        _canCount = false;
        _endTime.text = _minutesCount.ToString() + ":" + _secondsCount.ToString();
    }
    void SetVictoryPanel()
    {
        PauseManager.Instance.SetCanPause = false;
        _victoryPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }
    void CountTime()
    {
        if (!_canCount)
        {
            return;
        }
        _matchTime += Time.deltaTime;
        _secondsCount = (int)_matchTime;
        if (_secondsCount == 60)
        {
            print("aumento minuto");
            _matchTime = 0;
            _secondsCount = 0;
            _minutesCount++;
        }
        _timeTxt.text = _minutesCount.ToString() + ":" + _secondsCount.ToString();
    }
    public void UpdatePagesCollected(int pagesColected, int pagesToCollect)
    {
        print("Actualizo mi texto de paginas");
        _pagesCollectedText.text = "Pages: \n" + pagesColected.ToString() + " / " + pagesToCollect.ToString();
    }

    void GameOver()
    {
        print("Se Acabo");
        PauseManager.Instance.SetCanPause = false;
        _gameOvrPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        GMTestGameplay.OnGameOver -= GameOver;
        GMTestGameplay.OnGameOver -= StopCounting;
        GMTestGameplay.OnVictory -= SetVictoryPanel;
        GMTestGameplay.OnPageCollected -= UpdatePagesCollected;
    }
}

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

    float _matchTime;
    [SerializeField] int _secondsCount;
    [SerializeField] int _minutesCount;
    bool _canCount;

    void Start()
    {
        _canCount = true;
        PlayerStadistics.OnPyrDeath += StopCounting;
        GMTestGameplay.OnGameOver += GameOver;
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
        

        CountTime();
    }
    void StopCounting()
    {
        _canCount = false;
        _endTime.text = _minutesCount.ToString() + ":" + _secondsCount.ToString();
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

    void GameOver()
    {
        PauseManager.Instance.SetCanPause = false;
        _gameOvrPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerStadistics.OnPyrDeath -= StopCounting;
        GMTestGameplay.OnGameOver -= GameOver;
    }
}

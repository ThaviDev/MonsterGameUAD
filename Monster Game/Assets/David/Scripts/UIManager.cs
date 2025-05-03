using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] FloatSCOB _pyrHealth;
    [SerializeField] FloatSCOB _pyrStamina;

    [SerializeField] TMP_Text _timeTxt;
    [SerializeField] TMP_Text _endTime;
    [SerializeField] Slider _healthSlider;
    [SerializeField] Slider _staminaSlider;
    [SerializeField] GameObject _gameOvrPanel;
    [SerializeField] GameObject _gameplayPanel;
    [SerializeField] GameObject _pausePanel;

    float _matchTime;
    [SerializeField] int _secondsCount;
    [SerializeField] int _minutesCount;
    bool _canCount;
    bool _canPause;

    void Start()
    {
        _canPause = true;
        _canCount = true;
        PlayerStadistics.OnPyrDeath += StopCounting;
        GMTestGameplay.OnGameOver += GameOver;
        GMTestGameplay.OnPause += PauseUnpause;

    }
    void Update()
    {
        _healthSlider.value = _pyrHealth.SCOB_Value;
        _staminaSlider.value = _pyrStamina.SCOB_Value;

        CountTime();
    }
    void PauseUnpause(bool isPaused)
    {
        if (!_canPause) {
            return;
        }
        _pausePanel.SetActive(isPaused);
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
        print("End Game");
        _canPause = false;
        _gameOvrPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerStadistics.OnPyrDeath -= StopCounting;
        GMTestGameplay.OnGameOver -= GameOver;
        GMTestGameplay.OnPause -= PauseUnpause;
    }
}

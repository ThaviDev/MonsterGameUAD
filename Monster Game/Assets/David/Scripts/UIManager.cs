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
    [SerializeField] GameObject _gameOvrPanel;
    [SerializeField] GameObject _gameplayPanel;

    float _matchTime;
    [SerializeField] int _secondsCount;
    [SerializeField] int _minutesCount;
    bool _canCount;
    bool _canPause;

    void Start()
    {
        _canCount = true;
        PlayerStadistics.OnPyrDeath += StopCounting;
        GMTestGameplay.OnGameOver += GameOver;
    }
    void Update()
    {
        _healthSlider.value = _pyrHealth.SCOB_Value;

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
        bool canCountMinute = false;
        _matchTime += Time.deltaTime;
        _secondsCount = (int)_matchTime % 60;
        if (_secondsCount == 60)
        {
            _matchTime = 0;
            _secondsCount = 0;
            canCountMinute = true;
        }
        if (_secondsCount == 0 && canCountMinute) {
            _minutesCount++;
            canCountMinute = false;
        }
        _timeTxt.text = _minutesCount.ToString() + ":" + _secondsCount.ToString();
    }

    void GameOver()
    {
        print("End Game");
        _gameOvrPanel.SetActive(true);
        _gameplayPanel.SetActive(false);
    }
}

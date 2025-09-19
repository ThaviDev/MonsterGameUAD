using UnityEngine;

public class AlarmFunction : MonoBehaviour
{
    [SerializeField] MstrRandomPosSpawner _mstrSpawner;
    [SerializeField] MusicManager _musicManager;
    void Start()
    {
        
    }

    void Update()
    {
    }

    public void ActivateAlarmCall()
    {
        _mstrSpawner.ActivateHorde();
        _musicManager.SetMusic(1);
    }
}

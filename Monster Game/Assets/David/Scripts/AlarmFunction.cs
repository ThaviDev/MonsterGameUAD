using UnityEngine;

public class AlarmFunction : MonoBehaviour
{
    [SerializeField] MstrRandomPosSpawner _mstrSpawner;
    void Start()
    {
        
    }

    void Update()
    {
    }

    public void ActivateAlarmCall()
    {
        _mstrSpawner.ActivateHorde();
    }
}

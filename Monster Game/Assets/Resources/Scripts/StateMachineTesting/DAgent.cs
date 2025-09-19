using UnityEngine;

public class DAgent : MonoBehaviour
{
    public DStateMachine _dStateMachine;
    void Start()
    {
        if (_dStateMachine != null)
        {
            _dStateMachine.MyInitial();
        }
    }
    void Update()
    {
        if (_dStateMachine != null)
        {
            _dStateMachine.MyUpdate();
        }
    }
}

using UnityEngine;

public class DState : MonoBehaviour
{
    public DStateMachine _finitedStateMachine;
    public virtual void OnInitial()
    {
        Debug.Log("Holis");
    }

    public virtual DState OnUpdate()
    {
        return this;
    }

    public virtual void OnExit()
    {

    }
}

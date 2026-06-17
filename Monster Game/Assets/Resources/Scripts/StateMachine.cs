using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T Context;
    public State(T context) { Context = context; }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnTriggerEnter2D(Collider2D col) { }
}

public class StateMachine<T> where T : MonoBehaviour
{
    private State<T> _current;
    public State<T> Current => _current;

    public void ChangeState(State<T> newState)
    {
        if (newState == null) return;
        _current?.Exit();
        _current = newState;
        _current.Enter();
    }

    public void Update() => _current?.Update();
}
using UnityEngine;
public abstract class C_MonstState
{
    protected readonly C_MonsterMotor _monst;
    protected C_MonstState(C_MonsterMotor motor) => _monst = motor;
    public virtual void MyEnter() { }
    public virtual void MyExit() { }
    public virtual void MyUpdate() { }
    public virtual void MyTriggerColision(Collider2D other) { }
}

using UnityEngine;
public abstract class C_MonstState
{
    protected C_MonsterMotor Motor { get; private set; }
    protected C_MonstState(C_MonsterMotor motor) => Motor = motor;
    public virtual void MyEnter() { }
    public virtual void MyExit() { }
    public virtual void MyUpdate() { }
    public virtual void MyTriggerColision(Collider2D other) { }
}

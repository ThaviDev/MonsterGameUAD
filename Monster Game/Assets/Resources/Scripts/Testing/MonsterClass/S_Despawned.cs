using UnityEngine;

public class S_Despawned : C_MonstState
{
    public S_Despawned(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        Debug.Log("Estoy Despawneado");
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        _monst.RegenEnergy();
        _monst.RegenAgression();
    }
    public override void MyExit()
    {
        base.MyExit();
    }
    public override void MyTriggerColision(Collider2D other)
    {
        base.MyTriggerColision(other);
    }
}

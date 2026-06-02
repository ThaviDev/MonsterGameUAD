using UnityEngine;

public class S_Despawned : C_MonstState
{
    public S_Despawned(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        _monst.Visual.AnimDespawn();
        Debug.Log("Estoy Despawneado");
        _monst.Boid.SeekImpetu = 0;
        _monst.Boid.BoidMaxSpeed = 0;
        _monst.Boid.SeekTarget = null;
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

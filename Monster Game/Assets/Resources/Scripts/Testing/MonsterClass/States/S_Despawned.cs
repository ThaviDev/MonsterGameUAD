using UnityEngine;

public class S_Despawned : C_MonstState
{
    public S_Despawned(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        Motor.Visual.AnimDespawn();
        //Debug.Log("Estoy Despawneado");
        Motor.Boid.SeekImpetu = 0;
        Motor.Boid.BoidMaxSpeed = 0;
        Motor.Boid.SeekTarget = null;
        Motor.Boid.StopMovementTime = 1;
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.RegenEnergy();
        Motor.RegenAgression();
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

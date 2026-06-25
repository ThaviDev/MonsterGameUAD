using UnityEngine;

public class S_Spawned : C_MonstState
{
    public S_Spawned(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        if (Motor == null)
        {
            Debug.LogError("No tengo Motor");
        }
        Motor.Visual.AnimSpawn();
        //Debug.Log("Estoy Spawneado");

        Motor.Boid.SeekImpetu = 1;
        Motor.Boid.BoidMaxSpeed = Motor.SpeedCur;
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.DecreaseEnergy();
        Motor.RegenAgression();

        StablishState();
    }
    public virtual void StablishState()
    {
        if (Motor.Agresion >= Motor.AgressionChaseThreshold)
        {
            Motor.ChangeState(new S_Chasing(Motor));
        }
        else
        {
            Motor.ChangeState(new S_Stealthy(Motor));
        }
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

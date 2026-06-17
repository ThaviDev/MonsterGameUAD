using UnityEngine;
public class S_Chasing : C_MonstState
{
    public S_Chasing(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        Debug.Log("Estoy persiguiendo");
        Motor.Visual.AnimChase();

        Motor.Boid.SeekTarget = Motor.PredictionPoint;
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.DecreaseEnergy();
        Motor.DecreaseAgression();
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


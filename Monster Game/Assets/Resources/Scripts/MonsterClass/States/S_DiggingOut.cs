using UnityEngine;

public class S_DiggingOut : C_MonstState
{
    public S_DiggingOut(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        //Debug.Log("Estoy Digging Out");
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.DecreaseEnergy();
        Motor.RegenAgression();
    }
    public override void MyExit()
    {
        base.MyExit();
    }
}

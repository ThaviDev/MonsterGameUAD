using UnityEngine;

public class S_DiggingIn : C_MonstState
{
    public S_DiggingIn(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        //Debug.Log("Estoy Digging In");
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
}

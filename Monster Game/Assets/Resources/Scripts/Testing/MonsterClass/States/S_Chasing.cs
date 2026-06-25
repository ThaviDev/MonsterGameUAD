using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class S_Chasing : C_MonstState
{
    protected List<Vector3> m_ChasePath;
    public S_Chasing(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        //Debug.Log("Estoy persiguiendo");
        Motor.Visual.AnimChase();

        //Motor.Boid.SeekTarget = Motor.PredictionPoint;

        //m_ChasePath = Motor.PathFinder.GetPath(Motor.gameObject.transform.position, Motor.PredictionPoint.position);
        //Motor.Boid.Path = m_ChasePath;
        Motor.StartPathUpdater();
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.DecreaseEnergy();
        Motor.DecreaseAgression();
    }
    public override void MyExit()
    {
        Motor.StopPathUpdater();
        base.MyExit();
    }
    public override void MyTriggerColision(Collider2D other)
    {
        base.MyTriggerColision(other);
    }

}


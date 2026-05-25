using UnityEngine;

public class S_Spawned : C_MonstState
{
    public S_Spawned(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        _monst.Visual.AnimSpawn();
        Debug.Log("Estoy Spawneado");

        _monst.Boid.SeekImpetu = 1;
        _monst.Boid.BoidMaxSpeed = _monst.SpeedCur;
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        _monst.DecreaseEnergy();
        _monst.RegenAgression();
        
        if (_monst.Agresion >= _monst.AgressionChaseThreshold)
        {
            _monst.ChangeState(new S_Chasing(_monst));
        }
        else
        {
            _monst.ChangeState(new S_Stealthy(_monst));
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

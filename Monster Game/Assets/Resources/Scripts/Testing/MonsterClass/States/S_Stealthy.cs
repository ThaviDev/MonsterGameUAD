using UnityEngine;

public class S_Stealthy : C_MonstState
{
    public S_Stealthy(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        Debug.Log("Estoy sigiloso");
        _monst.Visual.AnimStealth();
        _monst.Boid.BoidMaxSpeed = _monst.SpeedCur/2;

        // Temporal, el monstruo debe seguir el punto de sigileza, no al jugador
        _monst.Boid.SeekTarget = _monst.PlayerMotor.gameObject.transform;

    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        _monst.DecreaseEnergy();
        _monst.RegenAgression();
    }
    public override void MyExit()
    {
        base.MyExit();
    }
    public override void MyTriggerColision(Collider2D other)
    {
        base.MyTriggerColision(other);
        // Aquí iría la lógica de colisión con el jugador, como infligir daño
    }

}

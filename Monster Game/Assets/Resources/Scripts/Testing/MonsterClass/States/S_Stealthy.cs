using UnityEngine;

public class S_Stealthy : C_MonstState
{
    public S_Stealthy(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        Motor.HasBeenFoundByPlayer = false;
        base.MyEnter();
        Debug.Log("Estoy sigiloso");
        Motor.Visual.AnimStealth();
        // Movimiento reducido a la mitad solo para ejemplo
        //Motor.Boid.BoidMaxSpeed = Motor.SpeedCur/2;

        /*
        // Temporal, el monstruo debe seguir el punto de sigileza, no al jugador
        Motor.Boid.SeekTarget = Motor.PlayerMotor.gameObject.transform;
        */
        Motor.StartPathUpdater();
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        Motor.DecreaseEnergy();
        Motor.RegenAgression();
        PlayerHasSeenMe();
    }
    public virtual void PlayerHasSeenMe()
    {
        if (Motor.HasBeenFoundByPlayer)
        {
            Motor.RandomizeSpawnAndDespawnValues();
            Motor.DecreasingEnergyWhenFound();
            Motor.ChangeState(new S_Despawned(Motor));
        }
    }
    public override void MyExit()
    {
        Motor.StopPathUpdater();
        base.MyExit();
    }
    public override void MyTriggerColision(Collider2D other)
    {
        base.MyTriggerColision(other);
        // Aquí iría la lógica de colisión con el jugador, como infligir daño
    }

}

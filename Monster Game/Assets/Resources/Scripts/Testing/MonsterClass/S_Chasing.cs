using UnityEngine;

public class S_Chasing : C_MonstState
{
    public S_Chasing(C_MonsterMotor motor) : base(motor) { }
    public override void MyEnter()
    {
        base.MyEnter();
        Debug.Log("Estoy persiguiendo");
    }
    public override void MyUpdate()
    {
        base.MyUpdate();
        _monst.DecreaseEnergy();
        _monst.DecreaseAgression();
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

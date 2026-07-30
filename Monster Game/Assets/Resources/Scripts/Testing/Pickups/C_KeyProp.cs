using UnityEngine;

public class C_KeyProp : C_PickUpProp
{
    // PickUp de llave es para desbloquear puertas y demás elementos
    
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnPyrPickup(C_PlayerMotor pyr)
    {
        base.OnPyrPickup(pyr);
    }
}

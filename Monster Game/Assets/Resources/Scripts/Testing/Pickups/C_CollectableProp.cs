using UnityEngine;

public class C_CollectableProp : C_PickUpProp
{
    // PickUp de Coleccionable sirve para desbloquear 
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnPyrPickup(C_PlayerMotor pyr)
    {
        base.OnPyrPickup(pyr);
    }
}

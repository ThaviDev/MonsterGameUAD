using UnityEngine;

public class C_PickUpProp : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // TryGetComponent es como get component pero evita la asignacion de un null o crear una variable para asignar a la condicion y la nueva funcion
        if (!other.TryGetComponent(out C_PlayerMotor pyrMotor))
        {
            return;
        }
        OnPyrPickup(pyrMotor);
    }
    protected virtual void OnPyrPickup(C_PlayerMotor pyr)
    {
        Debug.Log(pyr.gameObject + " recogio " + this.gameObject);
        Destroy(gameObject);
    }
}

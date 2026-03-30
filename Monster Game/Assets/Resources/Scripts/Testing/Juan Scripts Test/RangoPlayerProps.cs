using UnityEngine;

public class RangoPayerProps : MonoBehaviour
{
    public bool jugadorEnRango = false; // 👈 bool público

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            Debug.Log("Jugador en rango");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            Debug.Log("Jugador fuera de rango");
        }
    }
}
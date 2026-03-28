using UnityEngine;

public class PalancaScript : MonoBehaviour
{
    public bool activado = false;
    public KeyCode teclaActivar = KeyCode.L;

    [Header("Configuración")]
    public bool soloUnaVez = false; // 👈 decides en el editor

    public Animator anim;

    bool jugadorEnRango = false;

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        // Sincroniza animación inicial
        anim.SetBool("Activado", activado);
    }

    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(teclaActivar))
        {
            CambiarEstado();
        }
    }

    void CambiarEstado()
    {
        // Si es solo una vez y ya está activado, no hace nada
        if (soloUnaVez && activado)
            return;

        if (soloUnaVez)
        {
            activado = true;
        }
        else
        {
            activado = !activado;
        }

        anim.SetBool("Activado", activado);

        Debug.Log("Palanca: " + (activado ? "ACTIVADA" : "DESACTIVADA"));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }
}

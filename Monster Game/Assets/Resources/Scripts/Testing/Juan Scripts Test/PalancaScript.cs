using UnityEngine;

public class PalancaScript : MonoBehaviour
{
    public bool activado = false;
    public KeyCode teclaActivar = KeyCode.L;

    [Header("Configuración")]
    public bool soloUnaVez = false;
    public bool activarPorRango = false; // 👈 NUEVO

    public Animator anim;

    bool jugadorEnRango = false;

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        anim.SetBool("Activado", activado);
    }

    void Update()
    {
        // Modo tecla
        if (!activarPorRango && jugadorEnRango && Input.GetKeyDown(teclaActivar))
        {
            CambiarEstado();
        }
    }

    void CambiarEstado()
    {
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

            // Modo automático
            if (activarPorRango)
            {
                CambiarEstado();
            }
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

using UnityEngine;

public class DoorFunctionality : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioSource sonidoAbrir;
    [SerializeField] private AudioSource sonidoBloqueado;

    [Header("Configuración")]
    [SerializeField] private bool _isVertical;

    [Header("Modo puerta")]
    [SerializeField] private bool usarPalancas = false;

    [Header("Puerta normal (tiempo)")]
    [SerializeField] private float _closeTime;

    [Header("Puertas con palancas")]
    [SerializeField] private PalancaScript[] palancasNecesarias;

    private bool _openDoorCallOnce;
    private bool _closeDoorCallOnce;

    private bool puertaAbierta = false;

    void Awake()
    {
        _animator.SetBool("IsVertical", _isVertical);
    }

    void Update()
    {
        if (usarPalancas)
        {
            CheckPalancas();
        }
        else
        {
            // --- MODO NORMAL ---
            if (_closeTime > 0)
            {
                _closeTime -= Time.deltaTime;
            }

            if (_closeTime <= 0 && _closeDoorCallOnce)
            {
                CloseDoor();
            }

            if (_openDoorCallOnce)
            {
                OpenDoor();
            }
        }
    }

    // 🔁 Revisar palancas
    void CheckPalancas()
    {
        bool todasActivadas = true;

        foreach (var palanca in palancasNecesarias)
        {
            if (!palanca.activado)
            {
                todasActivadas = false;
                break;
            }
        }

        if (todasActivadas)
        {
            if (!puertaAbierta)
            {
                OpenDoor();
                ReproducirSonido(sonidoAbrir);
            }
        }
        else
        {
            if (puertaAbierta)
            {
                CloseDoor();
            }
        }
    }

    // 🚪 Abrir
    void OpenDoor()
    {
        puertaAbierta = true;
        _collider.enabled = false;
        _animator.SetBool("IsOpen", true);
    }

    // 🚪 Cerrar
    void CloseDoor()
    {
        puertaAbierta = false;
        _collider.enabled = true;
        _animator.SetBool("IsOpen", false);
    }

    // ⏱️ Llamada externa (modo normal)
    public void OpenDoorCall(float timeAmount)
    {
        if (usarPalancas) return;

        _closeTime = timeAmount;
        _openDoorCallOnce = true;
        _closeDoorCallOnce = true;

        ReproducirSonido(sonidoAbrir);
    }

    // 🔊 Cuando el jugador intenta abrir sin cumplir condiciones
    public void IntentarAbrir()
    {
        if (!usarPalancas) return;

        bool todasActivadas = true;

        foreach (var palanca in palancasNecesarias)
        {
            if (!palanca.activado)
            {
                todasActivadas = false;
                break;
            }
        }

        if (!todasActivadas)
        {
            ReproducirSonido(sonidoBloqueado);
        }
        else
        {
            // ya están todas → abrir (por si quieres usar input directo)
            OpenDoor();
            ReproducirSonido(sonidoAbrir);
        }
    }

    // 🔊 Helper para audio
    void ReproducirSonido(AudioSource audio)
    {
        if (audio != null && !audio.isPlaying)
        {
            audio.Play();
        }
    }
}

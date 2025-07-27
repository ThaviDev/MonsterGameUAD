using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private static PlayerInputs _instance;
    
    public static PlayerInputs Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<PlayerInputs>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("Input Manager");
                    _instance = singletonObject.AddComponent<PlayerInputs>();

                    // Opcional: Evitar que el objeto sea destruido al cambiar de escena.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    } 
    static PlayerInput _input;

    Vector2 _move;
    Vector2 _aim;
    bool _pause;
    bool _run;
    bool _breathe;
    bool _interact;
    private void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            _input = GetComponent<PlayerInput>(); // Obtiene el PlayerInput del mismo objeto
            DontDestroyOnLoad(gameObject); // Evitar que el objeto sea destruido al cambiar de escena.
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destruir instancias adicionales si ya existe una instancia.
        }
    }
    public Vector2 MovementVector { get { return _move; } }
    public Vector2 AimingVector { get { return _aim; } }
    public bool PauseBool { get { return _pause; } }
    public bool RuningBool { get { return _run; } }
    public bool BreathingBool { get { return _breathe; } }
    public bool InteractBool { get { return _interact; } }

    private void Update()
    {
        _move = OnMoveChange();
        _aim = OnAimChange();
        _pause = OnPausePressed();
        _breathe = OnBreathePressed();
        _run = OnRunPressed();
        _interact = OnInteractPressed();
    }

    public static Vector2 OnMoveChange()
    {
        return _input.actions.FindAction("Move").ReadValue<Vector2>();
        // .IsPressed(), .WasPressedThisFrame, .WasReleasedThisFrame
        // .ReadValue<Float>, .ReadValue<Vector2>
    }
    public static Vector2 OnAimChange()
    {
        return _input.actions.FindAction("Look").ReadValue<Vector2>();
    }
    public static bool OnPausePressed()
    {
        return _input.actions.FindAction("Pause").WasReleasedThisFrame();
    }
    public static bool OnRunPressed()
    {
        return _input.actions.FindAction("Sprint").IsPressed();
    }
    public static bool OnBreathePressed()
    {
        return _input.actions.FindAction("Jump").IsPressed();
    }
    public static bool OnInteractPressed()
    {
        return _input.actions.FindAction("Interact").WasReleasedThisFrame();
    }
}

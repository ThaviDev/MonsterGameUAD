using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    //private static PlayerInputs _instance;
    /*
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
    } */
    static PlayerInput _input;
    [SerializeField] PlayerInput _inputRef;

    Vector2 _move;
    Vector2 _aim;
    bool _pause;
    bool _run;
    private void Awake()
    {
        /*
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto sea destruido al cambiar de escena.
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destruir instancias adicionales si ya existe una instancia.
        } */
        _input = _inputRef;
    }
    public Vector2 MovementVector { get { return _move; } }
    public Vector2 AimingVector { get { return _aim; } }
    public bool PauseBool { get { return _pause; } }
    public bool RuningBool { get { return _run; } }

    private void Update()
    {
        _move = OnMoveChange();
        _aim = OnAimChange();
        _pause = OnPausePressed();
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
}

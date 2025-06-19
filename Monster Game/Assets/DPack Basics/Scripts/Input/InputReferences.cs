using UnityEngine;
using UnityEngine.InputSystem;

public class InputReferences : MonoBehaviour
{
    //private static InputReferences _instance;
    /*
    public static InputReferences Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<InputReferences>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("Input Manager");
                    _instance = singletonObject.AddComponent<InputReferences>();

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
    bool _attack;
    bool _interact;
    bool _crouch;
    bool _jump;
    bool _previus;
    bool _next;
    bool _sprint;
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
    public bool AttackBool { get { return _attack; } } 
    public bool InteractBool { get { return _interact; } }
    public bool CrouchBool { get { return _crouch; } }
    public bool JumpBool { get { return _jump; } }
    public bool PreviusBool { get { return _previus; } }
    public bool NextBool { get { return _next; } }
    public bool SprintBool { get { return _sprint; } }

    private void Update()
    {
        _move = OnMoveChange();
        _aim = OnAimChange();
        _attack = OnAttackPressed();
        _interact = OnInteractPressed();
        _crouch = OnCrouchPressed();
        _jump = OnJumpPressed();
        _previus = OnPreviusPressed();
        _next = OnNextPressed();
        _sprint = OnSprintPressed();
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
    public static bool OnAttackPressed()
    {
        return _input.actions.FindAction("Attack").IsPressed();
    }
    public static bool OnInteractPressed()
    {
        return _input.actions.FindAction("Interact").WasReleasedThisFrame();
    }
    public static bool OnCrouchPressed()
    {
        return _input.actions.FindAction("Crouch").IsPressed();
    }
    public static bool OnJumpPressed()
    {
        return _input.actions.FindAction("Jump").IsPressed();
    }
    public static bool OnPreviusPressed()
    {
        return _input.actions.FindAction("Previous").WasReleasedThisFrame();
    }
    public static bool OnNextPressed()
    {
        return _input.actions.FindAction("Next").WasReleasedThisFrame();
    }
    public static bool OnSprintPressed()
    {
        return _input.actions.FindAction("Sprint").IsPressed();
    }

}

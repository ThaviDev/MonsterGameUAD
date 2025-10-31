using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private static PlayerInputs _instance;
    static PlayerInput _input;

    [SerializeField] private InputActionAsset _inputAsset; // Asset asignable desde Inspector

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
                    DontDestroyOnLoad(singletonObject);

                    // Asignar valores indispensables
                    _instance.SetupInputComponent();
                }
            }
            return _instance;
        }
    } 

    Vector2 _move;
    Vector2 _aim;
    bool _interact;
    bool _useItem;
    bool _useApp;
    bool _run;
    bool _previousItem;
    bool _nextItem;
    bool _dash;
    bool _pause;
    bool _breathe;
    bool _cellphoneMenu;
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

    private void SetupInputComponent()
    {
        // 1. Obtener o crear PlayerInput
        if (!TryGetComponent(out _input))
        {
            _input = gameObject.AddComponent<PlayerInput>();
        }

        // 2. Configurar Input Action Asset
        if (_input.actions == null)
        {
            // Intento 1: Usar asset serializado (si se asignó en Inspector)
            if (_inputAsset != null)
            {
                _input.actions = _inputAsset;
            }
            // Intento 2: Cargar desde Resources
            else
            {
                _inputAsset = Resources.Load<InputActionAsset>("InputSystem_Actions");

                if (_inputAsset != null)
                {
                    _input.actions = _inputAsset;
                }
                else
                {
                    Debug.LogError($"Input Action Asset no encontrado en: Resources/InputSystem_Actions");
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                }
            }
        }

        // 3. Activar el sistema de input
        if (_input.actions != null && !_input.inputIsActive)
        {
            _input.ActivateInput();
        }
    }
    public Vector2 MovementVector { get { return _move; } }
    public Vector2 AimingVector { get { return _aim; } }
    public bool InteractBool { get { return _interact; } }
    public bool UseItemBool { get { return _useItem; } }
    public bool UseAppBool { get { return _useApp; } }
    public bool RuningBool { get { return _run; } }
    public bool PreviousItemBool { get { return _previousItem; } }
    public bool NextItemBool { get { return _nextItem; } }
    public bool DashBool { get { return _dash; } }
    public bool PauseBool { get { return _pause; } }
    public bool BreathingBool { get { return _breathe; } }
    public bool CellPhoneMenuBool { get { return _cellphoneMenu; } }

    private void Update()
    {
        _move = OnMoveChange();
        _aim = OnAimChange();
        _interact = OnInteractPressed();
        _useItem = OnItemPressed();
        _useApp = OnAppPressed();
        _run = OnRunPressed();
        _previousItem = OnPreviousItemPressed();
        _nextItem = OnNextItemPressed();
        _dash = OnDashPressed();
        _pause = OnPausePressed();
        _breathe = OnBreathePressing();
        _cellphoneMenu = OnCellPhoneMenuPressed();
    }

    static Vector2 OnMoveChange()
    {
        return _input.actions.FindAction("Move").ReadValue<Vector2>();
        // .IsPressed(), .WasPressedThisFrame, .WasReleasedThisFrame
        // .ReadValue<Float>, .ReadValue<Vector2>
    }
    static Vector2 OnAimChange()
    {
        return _input.actions.FindAction("Look").ReadValue<Vector2>();
    }
    static bool OnInteractPressed()
    {
        return _input.actions.FindAction("Interaction").WasReleasedThisFrame();
    }
    static bool OnItemPressed()
    {
        return _input.actions.FindAction("Use Item").WasReleasedThisFrame();
    }
    static bool OnAppPressed()
    {
        return _input.actions.FindAction("Use App").WasReleasedThisFrame();
    }
    static bool OnRunPressed()
    {
        return _input.actions.FindAction("Speed Control").WasReleasedThisFrame();
    }
    static bool OnPreviousItemPressed()
    {
        return _input.actions.FindAction("Previous").WasReleasedThisFrame();
    }
    static bool OnNextItemPressed()
    {
        return _input.actions.FindAction("Next").WasReleasedThisFrame();
    }
    static bool OnDashPressed()
    {
        return _input.actions.FindAction("Dash").WasReleasedThisFrame();
    }
    static bool OnPausePressed()
    {
        return _input.actions.FindAction("Pause").WasReleasedThisFrame();
    }
    static bool OnBreathePressing()
    {
        return _input.actions.FindAction("Breathing").IsPressed();
    }
    static bool OnCellPhoneMenuPressed()
    {
        return _input.actions.FindAction("Cellphone Menu").WasReleasedThisFrame();
    }

}

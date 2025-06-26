using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<PauseManager>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("Pause Manager");
                    _instance = singletonObject.AddComponent<PauseManager>();

                    // Opcional: Evitar que el objeto sea destruido al cambiar de escena.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    [SerializeField] GameObject _pauseUI_Obj;
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] GameObject _generalPanel;
    [SerializeField] bool _isPaused;
    bool _pauseKeyPressed;

    [SerializeField] bool _canPause;

    public bool SetCanPause { set { _canPause = value; } }
    public bool GetIsPaused { get { return _isPaused; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto sea destruido al cambiar de escena.
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destruir instancias adicionales si ya existe una instancia.
        }
    }

    void Start()
    {
        _isPaused = false;
        _canPause = true;
    }
    void Update()
    {
        if (_pauseKeyPressed)
        {
            PressedPauseKeyOrBtn();
        }
        if (!_canPause && _isPaused == true)
        {
            PressedPauseKeyOrBtn();
        } else
        {
            _pauseKeyPressed = PlayerInputs.OnPausePressed();
        }
    }
    public void PressedPauseKeyOrBtn()
    {
        _isPaused = !_isPaused;
        PauseLogic(_isPaused);

        if (_isPaused) {
            _pauseUI_Obj?.SetActive(true);
            _generalPanel.SetActive(true);
        } else
        {
            _generalPanel.SetActive(false);
            _optionsPanel?.SetActive(false);
            _pauseUI_Obj?.SetActive(false);
        }

        //OnPause?.Invoke(_isPaused);
    }
    void PauseLogic(bool currentPauseFlipFlop)
    {
        switch (currentPauseFlipFlop)
        {
            case true:
                Time.timeScale = 0;
                break;
            case false:
                Time.timeScale = 1;
                break;
        }
    }
    public void GoToMainTabBtn()
    {
        _generalPanel?.SetActive(true);
        _optionsPanel?.SetActive(false);
    }
    public void GoToOptionsTabBtn()
    {
        _generalPanel?.SetActive(false);
        _optionsPanel?.SetActive(true);
    }
}

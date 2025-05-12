using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GMTestGameplay : MonoBehaviour
{
    // Game Manager Test Gameplay
    private static GMTestGameplay _instance;
    public static GMTestGameplay Instance
    {
        get
        {
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena.
                _instance = FindAnyObjectByType<GMTestGameplay>();

                if (_instance == null)
                {
                    // Crear un nuevo GameObject con el script adjunto si no se encuentra ninguna instancia.
                    GameObject singletonObject = new GameObject("Game Manager");
                    _instance = singletonObject.AddComponent<GMTestGameplay>();

                    // Opcional: Evitar que el objeto sea destruido al cambiar de escena.
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    public static Action OnGameOver;
    public static Action<bool> OnPause;
    bool _didChangeScene;
    bool _pauseKeyPressed;
    bool _isPaused;

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
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "TestingMainMenu")
        {
            SetSceneToMainMenu();
        }
        if (sceneName == "TestingAStar")
        {
            SetScenetoGameplay();
        }
    }
    void Update()
    {
        _pauseKeyPressed = PlayerInputs.OnPausePressed();
        if (_pauseKeyPressed)
        {
            PressedPauseBtn();
        }
    }

    public void PressedPauseBtn()
    {
        // Esta funcion sirve para la tecla y para el boton de UI
        _isPaused = !_isPaused;
        print(_isPaused);
        PauseLogic(_isPaused);
        OnPause?.Invoke(_isPaused);
    }

    void PauseLogic(bool currentPauseFlipFlop)
    {
        print(currentPauseFlipFlop);
        switch (currentPauseFlipFlop) {
            case true:
                Time.timeScale = 0;
                break;
            case false:
                Time.timeScale = 1;
                break;
        }
    }

    void AtGameplay()
    {
        PlayerStadistics.OnPyrDeath += StartGameOverSequence;
    }
    void AtMainMenu()
    {

    }
    void ResetEvents()
    {
        Time.timeScale = 1;
        _isPaused = false;
        PlayerStadistics.OnPyrDeath -= StartGameOverSequence;
    }
    public void ChangeSceneToGameplay()
    {
        SceneManager.LoadScene("TestingAStar");
        SetScenetoGameplay();
    }
    public void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene("TestingMainMenu");
        SetSceneToMainMenu();
    }
    void SetScenetoGameplay()
    {
        ResetEvents();
        AtGameplay();
    }
    public void SetSceneToMainMenu()
    {
        ResetEvents();
        AtMainMenu();
    }

    void StartGameOverSequence()
    {
        OnGameOver?.Invoke();
    }
}

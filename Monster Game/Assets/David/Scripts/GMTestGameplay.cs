using System;
using TMPro.EditorUtilities;
using UnityEngine;

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

    void Start()
    {
        
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
        PauseLogic(_isPaused);
        OnPause?.Invoke(_isPaused);
    }

    void PauseLogic(bool currentPauseFlipFlop)
    {
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
        PlayerStadistics.OnPyrDeath -= StartGameOverSequence;
    }
    public void ChangeSceneToGameplay()
    {
        ResetEvents();
        //CambiarEscena
        AtGameplay();
    }
    public void ChangeSceneToMainMenu()
    {
        ResetEvents();
        //CambiarEscena
        AtMainMenu();
    }

    void StartGameOverSequence()
    {
        OnGameOver?.Invoke();
    }
}

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
    [SerializeField] string _menuSceneName;
    [SerializeField] string _levelSceneName;
    bool _didChangeScene;

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
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == _menuSceneName)
        {
            SetSceneToMainMenu();
        }
        if (sceneName == _levelSceneName)
        {
            SetScenetoGameplay();
        }
    }
    void Update()
    {

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
        PauseManager.Instance.SetCanPause = true;
        if (PauseManager.Instance.GetIsPaused)
        {
            PauseManager.Instance.PressedPauseKeyOrBtn();
        }
        PlayerStadistics.OnPyrDeath -= StartGameOverSequence;
    }
    public void ChangeSceneToGameplay()
    {
        SceneManager.LoadScene(_levelSceneName);
        SetScenetoGameplay();
    }
    public void ChangeSceneToMainMenu()
    {
        SceneManager.LoadScene(_menuSceneName);
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : Singletone<SceneChangeManager>
{
    public void LoadScene(string sceneName) // -- Cambio de escena por nombre --
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToMainMenu() // -- Metodos directos a las escenas --
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevelMJ()
    {
        SceneManager.LoadScene("LevelMJ");
    }

    public void QuitGame() // -- Salir del juego (por el momento queda aqui) --
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}

using UnityEngine;

public class MainMenuManagerTest : MonoBehaviour
{
    [SerializeField] GameObject _MenuPanel;
    [SerializeField] GameObject _TutorialPanel;
    void Start()
    {
        GoToMenu();
    }
    public void GoToMenu()
    {
        _MenuPanel.SetActive(true);
        _TutorialPanel.SetActive(false);
    }
    public void GoToTutorial()
    {
        _MenuPanel.SetActive(false);
        _TutorialPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

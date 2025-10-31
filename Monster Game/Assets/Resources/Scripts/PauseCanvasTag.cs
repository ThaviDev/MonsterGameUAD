using UnityEngine;
using UnityEngine.UI;

public class PauseCanvasTag : MonoBehaviour
{
    [SerializeField] Button _returnFromOpts;
    [SerializeField] Button _goToOpts;
    [SerializeField] Button _resumeGame;

    // Rest of Buttons are dealt with Game Manager

    private void Start()
    {
        _goToOpts.onClick.AddListener(PauseManager.Instance.GoToOptionsTabBtn);
        _returnFromOpts.onClick.AddListener(PauseManager.Instance.GoToMainTabBtn);
        _resumeGame.onClick.AddListener(PauseManager.Instance.PressedPauseKeyOrBtn);
    }
}

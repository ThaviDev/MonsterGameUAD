using UnityEngine;
using UnityEngine.EventSystems;

public class SetMySelectionUI : MonoBehaviour
{
    [SerializeField] EventSystem _eventSystem;
    /*
    [SerializeField] GameObject _gameObject;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetSelectedTo(_gameObject);
        }
    }*/
    public void SetSelectedTo(GameObject obj)
    {
        _eventSystem.SetSelectedGameObject(obj);
    }
}

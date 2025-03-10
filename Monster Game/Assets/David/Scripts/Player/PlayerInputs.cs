using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    static PlayerInput _input;
    [SerializeField] PlayerInput _inputRef;

    Vector2 _move;
    private void Awake()
    {
        _input = _inputRef;
    }
    public Vector2 MovementVector { get { return _move; } }

    private void Update()
    {
        _move = OnMoveChange();
    }

    public static Vector2 OnMoveChange()
    {
        return _input.actions.FindAction("Move").ReadValue<Vector2>();
    }
}

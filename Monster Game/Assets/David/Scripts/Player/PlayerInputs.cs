using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    static PlayerInput _input;
    [SerializeField] PlayerInput _inputRef;

    Vector2 _move;
    Vector2 _aim;
    private void Awake()
    {
        _input = _inputRef;
    }
    public Vector2 MovementVector { get { return _move; } }
    public Vector2 AimingVector { get { return _aim; } }

    private void Update()
    {
        _move = OnMoveChange();
        _aim = OnAimChange();
    }

    public static Vector2 OnMoveChange()
    {
        return _input.actions.FindAction("Move").ReadValue<Vector2>();
    }
    public static Vector2 OnAimChange()
    {
        return _input.actions.FindAction("Look").ReadValue<Vector2>();
    }
}

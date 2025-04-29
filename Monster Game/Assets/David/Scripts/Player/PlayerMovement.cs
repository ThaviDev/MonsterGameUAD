using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] PlayerInputs _inputs;
    private Rigidbody2D _rb;
    Vector2 _movementDirection;

    [SerializeField] float _normalSpeed = 5f;
    public float _curSpeed;

    FloatSCOB _pyrStamina;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _curSpeed = _normalSpeed;
    }
    void Update()
    {
        _movementDirection = PlayerInputs.OnMoveChange().normalized;
        //print(_movementDirection);
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = _movementDirection * _curSpeed;
    }
}

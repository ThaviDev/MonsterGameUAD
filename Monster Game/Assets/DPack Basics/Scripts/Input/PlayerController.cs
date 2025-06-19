using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] PlayerInputs _inputs;
    private Rigidbody2D _rb;
    Vector2 _movementDirection;
    private float _hasSelfControl;
    [SerializeField] float _movementSpeed;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _movementDirection = InputReferences.OnMoveChange().normalized;

        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (_hasSelfControl <= 0)
        {
            _rb.linearVelocity = _movementDirection * _movementSpeed;
        }
    }
}

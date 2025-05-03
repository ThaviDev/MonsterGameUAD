using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] PlayerInputs _inputs;
    private Rigidbody2D _rb;
    Vector2 _movementDirection;

    [SerializeField] float _normalSpeed = 5f;
    private float _curSpeed;

    private float _hasSelfControl;

    [SerializeField] FloatSCOB _pyrStamina;

    [SerializeField] float _damageKnockback = 5f;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _curSpeed = _normalSpeed;

        PlayerMotor.OnPyrHit += PlayerWasHit;
        PlayerStadistics.OnPyrDeath += PlayerDeath;
    }
    void Update()
    {
        _movementDirection = PlayerInputs.OnMoveChange().normalized;
        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }
        //print(_movementDirection);
    }
    private void FixedUpdate()
    {
        if (_hasSelfControl <= 0)
        {
            _rb.linearVelocity = _movementDirection * _curSpeed;
        }
    }

    private void PlayerWasHit(Collider2D otherCol)
    {
        _hasSelfControl = 1;
        print("Pushed Away by Hit");
        Transform myTrans = transform;
        Transform otherTrans = otherCol.transform;
        Vector2 direction = myTrans.position - otherTrans.position;
        Vector2 directionNormalized = direction.normalized;
        print(directionNormalized);

        _rb.AddForce(directionNormalized * _damageKnockback, ForceMode2D.Impulse);
    }

    private void PlayerDeath()
    {
        _hasSelfControl = 999;
    }
}

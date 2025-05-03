using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] PlayerInputs _inputs;
    private Rigidbody2D _rb;
    Vector2 _movementDirection;
    bool _isPressingRun;
    [SerializeField] float _staminaUseMult;
    [SerializeField] float _staminaRestMult;

    [SerializeField] float _maxStamina;

    [SerializeField] float _normalSpeed = 5f;
    [SerializeField] float _runSpeed = 10f;
    private float _curSpeed;

    private float _hasSelfControl;

    [SerializeField] FloatSCOB _pyrStamina;

    [SerializeField] float _damageKnockback = 5f;
    void Start()
    {
        _pyrStamina.SCOB_Value = _maxStamina;
        _rb = GetComponent<Rigidbody2D>();
        _curSpeed = _normalSpeed;

        PlayerMotor.OnPyrHit += PlayerWasHit;
        PlayerStadistics.OnPyrDeath += PlayerDeath;
    }
    void Update()
    {
        _movementDirection = PlayerInputs.OnMoveChange().normalized;
        _isPressingRun = PlayerInputs.OnRunPressed();
        print(_movementDirection);
        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }
        //print(_movementDirection);
        var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);

        if (_hasSelfControl <= 0 && _isPressingRun && absMovement > 0 && _pyrStamina.SCOB_Value > 0)
        {
            _curSpeed = _runSpeed;
            _pyrStamina.SCOB_Value -= Time.deltaTime * _staminaUseMult;
        } else
        {
            if (_pyrStamina.SCOB_Value < _maxStamina)
            {
                _pyrStamina.SCOB_Value += Time.deltaTime * _staminaRestMult;
            } else
            {
                _pyrStamina.SCOB_Value = _maxStamina;
            }
            _curSpeed = _normalSpeed;
        }
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

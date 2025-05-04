using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] PlayerInputs _inputs;
    private Rigidbody2D _rb;
    [SerializeField] PlayerStadistics _playerStats;
    Vector2 _movementDirection;
    bool _isPressingRun;

    private float _hasSelfControl;

    [SerializeField] FloatSCOB _pyrStamina;

    [SerializeField] float _damageKnockback = 5f;

    private int _movementStatus;
    public int GetMovementStatus
    {
        get { return _movementStatus; }
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        PlayerMotor.OnPyrHit += PlayerWasHit;
        PlayerStadistics.OnPyrDeath += PlayerDeath;
    }
    void Update()
    {
        _movementDirection = PlayerInputs.OnMoveChange().normalized;
        _isPressingRun = PlayerInputs.OnRunPressed();
        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }
        var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);

        if (_hasSelfControl <= 0 && _isPressingRun && absMovement > 0 && _pyrStamina.SCOB_Value > 0)
        {
            _movementStatus = 1; // Is Running
            //_curSpeed = _runSpeed;
            //_pyrStamina.SCOB_Value -= Time.deltaTime * _staminaUseMult;
        } else
        {
            _movementStatus = 0; // Is Walking
            /*
            if (_pyrStamina.SCOB_Value < _maxStamina)
            {
                _pyrStamina.SCOB_Value += Time.deltaTime * _staminaRestMult;
            } else
            {
                _pyrStamina.SCOB_Value = _maxStamina;
            }
            _curSpeed = _normalSpeed; */
        }
    }
    private void FixedUpdate()
    {
        if (_hasSelfControl <= 0)
        {
            _rb.linearVelocity = _movementDirection * _playerStats.GetSpeed;
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
    private void OnDestroy()
    {
        PlayerMotor.OnPyrHit -= PlayerWasHit;
        PlayerStadistics.OnPyrDeath -= PlayerDeath;
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    //[SerializeField] PlayerStadistics _playerStats;
    [SerializeField] C_PlayerStats _playerStats;

    [SerializeField] C_DashTrail _dashTrail;
    [SerializeField] GameObject _vfx_PlayerJump;

    Vector2 _movementDirection;
    bool _isPressingRun;
    bool _isPressingBreathe;
    float _moveStatusCooldown = 0;

    private float _hasSelfControl = 0;

    //[SerializeField] FloatSCOB _pyrStamina;
    [SerializeField] FloatSCOB _pyrHealth;

    [SerializeField] float _damageKnockback = 5f;

    [SerializeField] int _movementStatus;
    public int GetMovementStatus
    {
        get { return _movementStatus; }
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        PlayerMotor.OnPyrHit += PlayerWasHit;
        PlayerMotor.OnPyrDeath += PlayerNoMove;
        PlayerMotor.OnPanic += PlayerNoMove;
        PlayerMotor.OnRelax += PlayerCanMove;
    }
    void Update()
    {
        //test
        bool useItem = PlayerInputs.Instance.UseItemBool;
        if (useItem)
        {
            _dashTrail.m_startTrail = true;
            Instantiate(_vfx_PlayerJump, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            print("Uso Item Actual");
        }
        bool movetoPreviousItem = PlayerInputs.Instance.PreviousItemBool;
        if (movetoPreviousItem)
        {
            print("Me muevo al item anterior");
        }
        bool moveToNextItem = PlayerInputs.Instance.NextItemBool;
        if (moveToNextItem)
            print("Me muevo al siguiente item");

        //---
        _movementDirection = PlayerInputs.Instance.MovementVector.normalized;
        _isPressingRun = PlayerInputs.Instance.RuningBool;
        //---

        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }

        //print(_movementStatus);
        //var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);
        var absMovement = Mathf.Abs(_rb.linearVelocity.x) + Mathf.Abs(_rb.linearVelocity.y);


        //print(_moveStatusCooldown);
        if (_hasSelfControl <= 0 && _isPressingRun)
        {
            if (_movementStatus < 5 && _moveStatusCooldown <= 0)
            {
                _movementStatus++;
                _moveStatusCooldown = 0.3f;
            }
        } else
        {
            if (_moveStatusCooldown > 0)
            {
                _moveStatusCooldown -= Time.deltaTime;
            }
        }
        if (absMovement < 0.1f)
        {
            _movementStatus = 0;
        } else if (absMovement > 0.1f && _movementStatus == 0)
        {
            _movementStatus = 1;
        }
        /*
        if (_hasSelfControl <= 0 && _isPressingRun && absMovement > 2)
        {
            //print(_rb.linearVelocity);
            if (_pyrStamina.SCOB_Value > 0)
            {
                _movementStatus = 1; // Is Running With Stamina
            }
            else if (_pyrHealth.SCOB_Value > 20)
            {
                _movementStatus = 2; // Is Running With Health
            } else
            {
                if (_movementStatus == 2 && _pyrHealth.SCOB_Value < 20 && _pyrHealth.SCOB_Value > 19)
                {
                    _pyrHealth.SCOB_Value = 20;
                }
                _movementStatus = 0; // Is Walking
            }
            //_curSpeed = _runSpeed;
            //_pyrStamina.SCOB_Value -= Time.deltaTime * _staminaUseMult;
        } 
        else
        {
            _movementStatus = 0; // Is Walking
        }
        */


    }
    private void FixedUpdate()
    {
        if (_hasSelfControl <= 0 && _movementDirection != Vector2.zero)
        {
            _rb.AddForce(_movementDirection * _playerStats.GetCurrentAcceleration, ForceMode2D.Force);

            if (_rb.linearVelocity.magnitude > _playerStats.GetCurrentSpeedGoal)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _playerStats.GetCurrentSpeedGoal;
            }
        }
        else
        {
            _rb.AddForce(_rb.linearVelocity * -_playerStats.GetCurrentDeceleration, ForceMode2D.Force);
        }
        /*
        if (_hasSelfControl <= 0)
        {
            _rb.linearVelocity = _movementDirection * _playerStats.GetSpeed;
        }
        */
    }

    private void PlayerWasHit(Collider2D otherCol)
    {
        _hasSelfControl += 1;
        Transform myTrans = transform;
        Transform otherTrans = otherCol.transform;
        Vector2 direction = myTrans.position - otherTrans.position;
        Vector2 directionNormalized = direction.normalized;
        print(directionNormalized);

        _rb.AddForce(directionNormalized * _damageKnockback, ForceMode2D.Impulse);
    }

    private void PlayerNoMove()
    {
        _hasSelfControl = 999;
    }
    private void PlayerCanMove()
    {
        _hasSelfControl = 0;
    }
    private void OnDestroy()
    {
        PlayerMotor.OnPyrHit -= PlayerWasHit;
        PlayerMotor.OnPyrDeath -= PlayerNoMove;
        PlayerMotor.OnPanic -= PlayerNoMove;
        PlayerMotor.OnRelax -= PlayerCanMove;
    }
}

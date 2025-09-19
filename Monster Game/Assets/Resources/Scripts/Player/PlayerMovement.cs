using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] PlayerStadistics _playerStats;
    Vector2 _movementDirection;
    bool _isPressingRun;
    bool _isPressingBreathe;
    float _moveStatusCooldown = 0;

    private float _hasSelfControl;

    [SerializeField] FloatSCOB _pyrStamina;
    [SerializeField] FloatSCOB _pyrHealth;

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
        //test
        bool useItem = PlayerInputs.Instance.UseItemBool;
        if (useItem)
        {
            print("Uso Item Actual");
        }
        bool useApp = PlayerInputs.Instance.UseAppBool;
        if (useApp)
        {
            print("Uso Applicacion Actual");
        }
        bool movetoPreviousItem = PlayerInputs.Instance.PreviousItemBool;
        if (movetoPreviousItem)
        {
            print("Me muevo al item anterior");
        }
        bool moveToNextItem = PlayerInputs.Instance.NextItemBool;
        if (moveToNextItem)
            print("Me muevo al siguiente item");
        bool cellPhoneMenu = PlayerInputs.Instance.CellPhoneMenuBool;
        if (cellPhoneMenu)
            print("Activo o desactivo celular");

        //---
        _movementDirection = PlayerInputs.Instance.MovementVector.normalized;
        _isPressingRun = PlayerInputs.Instance.RuningBool;
        _isPressingBreathe = PlayerInputs.Instance.BreathingBool;
        //---
        if (_isPressingBreathe)
            print("empiezo a respirar");

        if (_hasSelfControl > 0)
        {
            _hasSelfControl -= Time.deltaTime;
        }

        print(_movementStatus);
        //var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);
        var absMovement = Mathf.Abs(_rb.linearVelocity.x) + Mathf.Abs(_rb.linearVelocity.y);


        if (_hasSelfControl <= 0 && _isPressingRun && _pyrHealth.SCOB_Value > 20)
        {
            if (_movementStatus <= 3 && _moveStatusCooldown <= 0)
            {
                _movementStatus++;
            }
            _moveStatusCooldown = 0.3f;
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

            if (_rb.linearVelocity.magnitude > _playerStats.GetCurrentMaxSpeed)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _playerStats.GetCurrentMaxSpeed;
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
        _hasSelfControl = 1;
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

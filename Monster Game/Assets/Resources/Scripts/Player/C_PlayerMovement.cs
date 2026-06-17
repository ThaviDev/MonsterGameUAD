using UnityEngine;

public class C_PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    //[SerializeField] PlayerStadistics _playerStats;
    [SerializeField] C_PlayerStats m_playerStats;

    [SerializeField] C_DashTrail m_dashTrail;
    [SerializeField] GameObject m_vfx_PlayerJump;

    Vector2 m_movementDirection;
    bool m_isPressingRun;
    bool m_isPressingBreathe;
    float m_moveStatusCooldown = 0;

    private float m_hasSelfControl = 0;

    //[SerializeField] FloatSCOB _pyrStamina;
    //[SerializeField] FloatSCOB m_pyrHealth;

    // Multiplica el danio por el cual el jugador es golpeado
    [SerializeField] float m_damageKnockbackMultiplier = 2f;

    /* Determina el estado de velocidad del jugador
     * 0 = Idle
     * 1 = Caminata Normal
     * 2 = Trotar
     * 3 = Correr
     */
    [SerializeField] int m_movementStatus;
    public int GetMovementStatus { get { return m_movementStatus; } }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        C_PlayerMotor.OnPyrHit += PlayerWasHit;

        C_PlayerMotor.OnPyrDeath += PlayerNoMove;
        C_PlayerMotor.OnPanic += PlayerNoMove;
        C_PlayerMotor.OnRelax += PlayerCanMove;
    }
    void Update()
    {
        //test
        bool Dash = PlayerInputs.Instance.DashBool;
        if (Dash)
        {
            m_dashTrail.m_startTrail = true;
            Instantiate(m_vfx_PlayerJump, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
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
        m_movementDirection = PlayerInputs.Instance.MovementVector.normalized;
        m_isPressingRun = PlayerInputs.Instance.RuningBool;
        //---

        if (m_hasSelfControl > 0)
        {
            m_hasSelfControl -= Time.deltaTime;
        }

        //print(_movementStatus);
        //var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);
        var absMovement = Mathf.Abs(_rb.linearVelocity.x) + Mathf.Abs(_rb.linearVelocity.y);


        //print(_moveStatusCooldown);
        if (m_hasSelfControl <= 0 && m_isPressingRun)
        {
            if (m_movementStatus < 5 && m_moveStatusCooldown <= 0)
            {
                m_movementStatus++;
                m_moveStatusCooldown = 0.3f;
            }
        } else
        {
            if (m_moveStatusCooldown > 0)
            {
                m_moveStatusCooldown -= Time.deltaTime;
            }
        }
        if (absMovement < 0.1f)
        {
            m_movementStatus = 0;
        } else if (absMovement > 0.1f && m_movementStatus == 0)
        {
            m_movementStatus = 1;
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
        if (m_hasSelfControl <= 0 && m_movementDirection != Vector2.zero)
        {
            _rb.AddForce(m_movementDirection * m_playerStats.GetCurrentAcceleration, ForceMode2D.Force);

            if (_rb.linearVelocity.magnitude > m_playerStats.GetCurrentSpeedGoal)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * m_playerStats.GetCurrentSpeedGoal;
            }
        }
        else
        {
            _rb.AddForce(_rb.linearVelocity * -m_playerStats.GetCurrentDeceleration, ForceMode2D.Force);
        }
        /*
        if (_hasSelfControl <= 0)
        {
            _rb.linearVelocity = _movementDirection * _playerStats.GetSpeed;
        }
        */
    }

    private void PlayerWasHit(Collider2D otherCol, float damageAmount)
    {
        m_hasSelfControl += 1;
        Transform myTrans = transform;
        Transform otherTrans = otherCol.transform;
        Vector2 direction = myTrans.position - otherTrans.position;
        Vector2 directionNormalized = direction.normalized;
        print(directionNormalized);

        _rb.AddForce(directionNormalized * (m_damageKnockbackMultiplier * damageAmount), ForceMode2D.Impulse);
    }

    private void PlayerNoMove()
    {
        m_hasSelfControl = 999;
    }
    private void PlayerCanMove()
    {
        m_hasSelfControl = 0;
    }
    private void OnDestroy()
    {
        C_PlayerMotor.OnPyrHit -= PlayerWasHit;
        C_PlayerMotor.OnPyrDeath -= PlayerNoMove;
        C_PlayerMotor.OnPanic -= PlayerNoMove;
        C_PlayerMotor.OnRelax -= PlayerCanMove;
    }
}

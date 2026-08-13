using UnityEngine;

public class C_PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_RB;
    //[SerializeField] PlayerStadistics _playerStats;
    [SerializeField] C_PlayerStats m_playerStats;

    [SerializeField] C_DashTrail m_dashTrail;
    [SerializeField] GameObject m_vfx_PlayerJump;

    Vector2 m_movementDirection;
    bool m_isPressingRun;
    bool m_isDashing;
    bool m_isPressingBreathe;

    private float m_CantMoveTime = 0;

    //[SerializeField] FloatSCOB _pyrStamina;
    //[SerializeField] FloatSCOB m_pyrHealth;

    // Multiplica el danio por el cual el jugador es golpeado
    [SerializeField] float m_damageKnockbackMultiplier = 2f;

    [SerializeField] int m_movementStatus;
    float m_moveStatusCooldown = 0;
    [SerializeField] int m_movementStatusTotal = 3;
    public int GetMovementStatus { get { return m_movementStatus; } }

    [SerializeField] float m_DashImpulse;
    [SerializeField] float m_DashTime;
    [SerializeField] float m_DashStaminaUse;
    private float m_DashCurTime;
    private void Awake()
    {
        C_PlayerMotor.OnPyrHit += PyrWasHitEvent;
        GMTestGameplay.OnGameOver += PlayerNoMove;
        C_PlayerMotor.OnPanic += PlayerNoMove;
        C_PlayerMotor.OnRelax += PlayerCanMove;
    }
    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    void PyrWasHitEvent(Collider2D collider, float damage, float fear, float stunDuration, float knockbackForce)
    {
        PlayerWasHit(collider, knockbackForce);
    }
    void Update()
    {
        //test
        bool movetoPreviousItem = PlayerInputs.Instance.PreviousItemBool;
        if (movetoPreviousItem)
        {
            print("Me muevo al item anterior");
        }
        bool moveToNextItem = PlayerInputs.Instance.NextItemBool;
        if (moveToNextItem)
            print("Me muevo al siguiente item");



        if (m_CantMoveTime > 0)
        {
            m_CantMoveTime -= Time.deltaTime;
        } else
        {
            HandleMovement();
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

    public void IncreaseMoveStatus()
    {
        if (m_movementStatus != m_movementStatusTotal)
        {
            m_movementStatus++;
            m_moveStatusCooldown = 0.3f;
        }
    }
    public void DecreaseMoveStatus()
    {
        if (m_movementStatus > 0)
        {
            m_movementStatus--;
        }
    }
    private void HandleMovement()
    {
        //---
        m_movementDirection = PlayerInputs.Instance.MovementVector.normalized;
        //m_isPressingRun = PlayerInputs.Instance.RuningBool;
        //m_isDashing = PlayerInputs.Instance.DashBool;
        m_isDashing = PlayerInputs.Instance.RuningBool;
        //---

        //print(_movementStatus);
        //var absMovement = Mathf.Abs(_movementDirection.x) + Mathf.Abs(_movementDirection.y);
        var absMovement = Mathf.Abs(m_RB.linearVelocity.x) + Mathf.Abs(m_RB.linearVelocity.y);

        //print(_moveStatusCooldown);
        if (m_isDashing)
        {
            if (m_moveStatusCooldown <= 0)
            {
                m_DashCurTime = m_DashTime;
                m_dashTrail.m_startTrail = true;
                Instantiate(m_vfx_PlayerJump, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
                IncreaseMoveStatus();
                m_playerStats.UseStamina(m_DashStaminaUse);
            }
        }
        else
        {
            m_DashCurTime -= Time.deltaTime;
            if (m_moveStatusCooldown > 0)
            {
                m_moveStatusCooldown -= Time.deltaTime;
            }
        }
        if (absMovement < 0.1f)
        {
            m_movementStatus = 0;
        }
        else if (absMovement > 0.1f && m_movementStatus == 0)
        {
            m_movementStatus = 1;
        }
    }
    private void FixedUpdate()
    {
        if (m_DashCurTime > 0) {
            // DASH LOGIC
            if (m_movementDirection != Vector2.zero && m_CantMoveTime <= 0)
            {
                m_RB.AddForce(m_movementDirection * m_DashImpulse, ForceMode2D.Impulse);
            }
            return;
        }
        if (m_CantMoveTime <= 0 && m_movementDirection != Vector2.zero)
        {
            m_RB.AddForce(m_movementDirection * m_playerStats.GetCurrentAcceleration, ForceMode2D.Force);

            if (m_RB.linearVelocity.magnitude > m_playerStats.GetCurrentSpeedGoal)
            {
                m_RB.linearVelocity = m_RB.linearVelocity.normalized * m_playerStats.GetCurrentSpeedGoal;
            }
        }
        else
        {
            m_RB.AddForce(m_RB.linearVelocity * -m_playerStats.GetCurrentDeceleration, ForceMode2D.Force);
        }
    }

    private void PlayerWasHit(Collider2D otherCol, float knockbackForce)
    {
        m_CantMoveTime += 1;
        Transform myTrans = transform;
        Transform otherTrans = otherCol.transform;
        Vector2 direction = myTrans.position - otherTrans.position;
        Vector2 directionNormalized = direction.normalized;
        Debug.Log("PlayerWasHit in direction: " + directionNormalized);

        m_RB.AddForce(directionNormalized * (m_damageKnockbackMultiplier * knockbackForce), ForceMode2D.Impulse);
    }

    private void PlayerNoMove()
    {
        Debug.Log("PlayerNoMove");
        m_movementStatus = 0;
        m_CantMoveTime = 999;
        m_RB.linearVelocity = Vector3.zero;
    }
    private void PlayerCanMove()
    {
        m_CantMoveTime = 0;
    }
    private void OnDestroy()
    {
        C_PlayerMotor.OnPyrHit -= PyrWasHitEvent;
        GMTestGameplay.OnGameOver -= PlayerNoMove;
        C_PlayerMotor.OnPanic -= PlayerNoMove;
        C_PlayerMotor.OnRelax -= PlayerCanMove;
    }
}

using System;
using UnityEngine;

public class C_PlayerMotor : MonoBehaviour
{
    public static Action<Collider2D> OnPyrHit;
    public static Action OnPanic;
    public static Action OnRelax;
    public static Action OnPyrDeath;

    // Temporal por clase de VFX
    public GameObject m_vfx_MonsterScream;

    // Temporal referencia de PlayerStats para obtener BMP
    // Luego hay que cambiar esa clase a un SCOB
    public C_PlayerStats m_playerStats;

    // Temporal FeedbackQueue del sprite del jugador
    public SpriteRenderer m_sprite;

    //[SerializeField] private bool m_IsInPanic;

    [SerializeField] private bool m_IsInvincible;

    Collider2D _yCol;

    private PlayerState m_currentState;
    void Start()
    {
        if (m_IsInvincible)
        {
            ChangeState(new InvincibleState(this));
        }
        else
        {
            ChangeState(new RegularState(this));
        }
    }

    void Update()
    {
        print(m_currentState);
        // Esto actualiza constantemente cualquiera que sea el estado actual del jugador
        m_currentState?.MyUpdate();

        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPyrHit?.Invoke();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        m_currentState?.MyTriggerColision(otherCol);
        /*
        if (otherCol.gameObject.layer == 6) // Monster Layer
        {
            OnPyrHit?.Invoke(otherCol);
            if (m_IsInPanic)
            {
                OnPyrDeath?.Invoke();
            }
            else if (m_playerStats.GetBPM >= 150)
            {
                OnPyrDeath?.Invoke();
            }

            // Temporal para clase de VFX
            //Instantiate(m_vfx_MonsterScream, new Vector3(otherCol.transform.position.x, otherCol.transform.position.y + 1.6f), Quaternion.identity);
        }
        */
    }

    private void ChangeState(PlayerState newState)
    {
        if (newState == null)
        {
            return;
        }
        m_currentState?.MyExit();
        m_currentState = newState;
        m_currentState.MyEnter();
    }

    private void CheckIfDeath()
    {
        if (m_playerStats.GetBPM >= 150)
        {
            OnPyrDeath?.Invoke();
        }
    }

    // Estado Base
    private abstract class PlayerState
    {
        protected readonly C_PlayerMotor _PM;
        protected PlayerState(C_PlayerMotor motor) => _PM = motor;

        public virtual void MyEnter() { }
        public virtual void MyExit() { }
        public virtual void MyUpdate() { }
        public virtual void MyTriggerColision(Collider2D other) { }
    }
    private class RegularState : PlayerState
    {
        public RegularState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            OnRelax?.Invoke();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            if (_PM.m_playerStats.GetBPM >= _PM.m_playerStats.GetPanicThreshold)
            {
                //m_IsInPanic = true;
                _PM.ChangeState(new PanicState(_PM));
                //OnPanic?.Invoke();
                // Temporal Feedback Queue
                //m_sprite.color = Color.blue;
            }
        }
        public override void MyTriggerColision(Collider2D otherCol)
        {
            base.MyTriggerColision(otherCol);
            if (otherCol.gameObject.layer == 6) // Monster Layer
            {
                OnPyrHit?.Invoke(otherCol);
                _PM.CheckIfDeath();
                /*
                if (_PM.m_playerStats.GetBPM >= 150)
                {
                    OnPyrDeath?.Invoke();
                }*/
            }
        }
    }
    private class PanicState : PlayerState
    {
        public PanicState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            OnPanic?.Invoke();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            //if (_PM.m_playerStats.GetBPM <= _PM.m_playerStats.GetRelaxThreshold && _PM.m_isInPanic)

            if (_PM.m_playerStats.GetBPM <= _PM.m_playerStats.GetRelaxThreshold)
            {
                //m_IsInPanic = false;
                _PM.ChangeState(new RegularState(_PM));
                //OnRelax?.Invoke();
                // Temporal Feedback Queue
                //_PM.m_sprite.color = Color.white;
            }
        }
        public override void MyTriggerColision(Collider2D otherCol)
        {
            base.MyTriggerColision(otherCol);
            if (otherCol.gameObject.layer == 6) // Monster Layer
            {
                OnPyrHit?.Invoke(otherCol);
                OnPyrDeath?.Invoke();
            }
        }
    }
    private class InvincibleState : PlayerState
    {
        public InvincibleState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
    private class StunnedState : PlayerState
    {
        public StunnedState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
}

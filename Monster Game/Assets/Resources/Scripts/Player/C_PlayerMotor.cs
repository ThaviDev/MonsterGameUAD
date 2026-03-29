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
    public C_PlayerStats m_playerStats;

    // Temporal FeedbackQueue del sprite del jugador
    public SpriteRenderer m_sprite;

    [SerializeField] private bool m_IsInvincible;

    private PlayerState m_CurrentState;

    private C_Monster m_MonsterThatGrabbed;
    private Vector2 m_PlayerGrabbedPos;
    private bool m_CanMashOutOfGrab;
    [SerializeField] private int m_GrabMashCount;

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
        //print(m_CurrentState);
        // Esto actualiza constantemente cualquiera que sea el estado actual del jugador
        m_CurrentState?.MyUpdate();



        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPyrHit?.Invoke();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        m_CurrentState?.MyTriggerColision(otherCol);
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
        m_CurrentState?.MyExit();
        m_CurrentState = newState;
        m_CurrentState.MyEnter();
    }

    private void CheckIfDeath()
    {
        if (m_playerStats.GetBPM >= 150)
        {
            OnPyrDeath?.Invoke();
        }
    }

    // Detonador de Estado Agarrado, se llama desde el script del monstruo que agarra al jugador
    public void GetGrabbed(C_Monster monsterThatGrabbed, Vector2 playerGrabbedPos, bool canMashOut)
    {
        m_PlayerGrabbedPos = playerGrabbedPos;
        m_MonsterThatGrabbed = monsterThatGrabbed;
        ChangeState(new GrabedState(this));
        m_CanMashOutOfGrab = canMashOut;
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
            // Temporal Feedback Queue
            _PM.m_sprite.color = Color.white;
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
            // Temporal Feedback Queue
            _PM.m_sprite.color = Color.blue;
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
    private class GrabedState : PlayerState
    {
        public GrabedState(C_PlayerMotor motor) : base(motor) { }
        private int m_CurMashCount;
        public override void MyEnter()
        {
            base.MyEnter();
            // Temporal Feedback Queue
            _PM.m_sprite.color = Color.red;
            m_CurMashCount = _PM.m_GrabMashCount;
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            //_PM.gameObject.transform.position = _PM.m_MonsterThatGrabbed.PlayerGrabbedPosition + (Vector2)_PM.m_MonsterThatGrabbed.transform.position;
            _PM.transform.position = _PM.m_PlayerGrabbedPos + (Vector2)_PM.m_MonsterThatGrabbed.transform.position;
            if (_PM.m_CanMashOutOfGrab)
            {
                KeyMeshingMinigame();
            }
        }
        private void KeyMeshingMinigame()
        {
            if (PlayerInputs.Instance.InteractAndPickUpItemBool)
            {
                m_CurMashCount--;
                if (m_CurMashCount <= 0)
                {
                    //_PM.m_MonsterThatGrabbed.ReleaseGrab();
                    // Accion 1 es liberarse del agarre
                    _PM.m_MonsterThatGrabbed.PlayerAction1_Bool = true;
                    _PM.ChangeState(new RegularState(_PM));
                }
            }
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
}

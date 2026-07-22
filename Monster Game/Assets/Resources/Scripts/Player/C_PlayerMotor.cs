using System;
using UnityEngine;
using UnityEngine.Events;

public class C_PlayerMotor : MonoBehaviour
{
    public static Action<Collider2D, float> OnPyrHit;
    public static Action OnPanic;
    public static Action OnRelax;
    public static Action OnPyrDeath;
    public static Action<C_MonsterMotor, Vector2, bool> OnGetGrabbed;
    public static Action<C_MonsterMotor, float> OnScreamer;

    [SerializeField] private C_PlayerStats m_PlayerStats;
    public C_PlayerStats PlayerStats { get { return m_PlayerStats; } set { m_PlayerStats = value; } }

    /*
    // Temporal por clase de VFX
    public GameObject m_vfx_MonsterScream;


    // Temporal FeedbackQueue del sprite del jugador
    public SpriteRenderer m_sprite;
    */

    [SerializeField] private bool m_IsInvincible;

    private PlayerState m_CurrentState;

    [SerializeField] private int m_GrabMashCount;
    private float m_CurStunDuration;

    void Start()
    {
        m_PlayerStats = GetComponent<C_PlayerStats>();
        if (m_IsInvincible)
        {
            ChangeState(new InvincibleState(this));
        }
        else
        {
            ChangeState(new RegularState(this));
        }
        OnGetGrabbed += GotGrabbed;
        OnPyrDeath += KillPlayerRegardless;
        OnScreamer += GotScreamedAt;
        //OnScreamer += (monster, fearAmount) => m_PlayerStats.RecieveFear(fearAmount);
    }

    void GotScreamedAt(C_MonsterMotor monster, float fearAmount)
    {
        print("AAAAH ME ASUSTO" + monster);
        // ANIADIR LUEGO FUNCIONALIDAD DE KNOCKBACK
        m_PlayerStats.RecieveFear(fearAmount);
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
        if (otherCol.gameObject.GetComponent<TAG_PageCollectable>() != null)
        {
            print ("Page Collected");
            Destroy(otherCol.gameObject);
            GMTestGameplay.Instance.CollectPage(1);
        }
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
        if (m_PlayerStats.GetBPM >= 150)
        {
            OnPyrDeath?.Invoke();
        }
    }

    private void GotGrabbed(C_MonsterMotor monsterThatGrabbed, Vector2 playerGrabbedPos, bool canMashOut)
    {
        m_CurrentState?.MyExit();
        m_CurrentState = new GrabedState(this, monsterThatGrabbed, playerGrabbedPos, canMashOut);
        m_CurrentState.MyEnter();
    }
    /*
    public void GetGrabbed(C_MonsterMotor monsterThatGrabbed, Vector2 playerGrabbedPos, bool canMashOut)
    {
        m_PlayerGrabbedPos = playerGrabbedPos;
        m_MonsterThatGrabbed = monsterThatGrabbed;
        ChangeState(new GrabedState(this));
        m_CanMashOutOfGrab = canMashOut;
    } 
    */

    private void KillPlayerRegardless()
    {
        ChangeState(new DeadState(this));
    }

    // Estado Base
    private abstract class PlayerState
    {
        protected readonly C_PlayerMotor _PM;
        protected PlayerState(C_PlayerMotor motor) => _PM = motor;
        public virtual void MyEnter() { }
        public virtual void MyUpdate() { }
        public virtual void MyExit() { }
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
            //_PM.m_sprite.color = Color.white;
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            if (_PM.PlayerStats.GetBPM >= _PM.PlayerStats.GetPanicThreshold)
            {
                //m_IsInPanic = true;
                _PM.ChangeState(new PanicState(_PM));
                //OnPanic?.Invoke();
                // Temporal Feedback Queue
                //m_sprite.color = Color.blue;
            }
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D otherCol)
        {
            base.MyTriggerColision(otherCol);
            /*
            if (otherCol.gameObject.layer == 6) // Monster Layer
            {
                OnPyrHit?.Invoke(otherCol, otherCol.gameObject.GetComponent<C_Monster>().GroundHitDamage);
                _PM.CheckIfDeath();
            }
            */
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
            //_PM.m_sprite.color = Color.blue;
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            //if (_PM.m_playerStats.GetBPM <= _PM.m_playerStats.GetRelaxThreshold && _PM.m_isInPanic)

            if (_PM.m_PlayerStats.GetBPM <= _PM.m_PlayerStats.GetRelaxThreshold)
            {
                //m_IsInPanic = false;
                _PM.ChangeState(new RegularState(_PM));
                //OnRelax?.Invoke();
                // Temporal Feedback Queue
                //_PM.m_sprite.color = Color.white;
            }
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D otherCol)
        {
            base.MyTriggerColision(otherCol);
            if (otherCol.gameObject.layer == 6) // Monster Layer
            {
                // Luego hay que detectar mejor esto
                OnPyrHit?.Invoke(otherCol, otherCol.gameObject.GetComponent<C_Monster>().GroundHitDamage);
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
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
    private class DeadState : PlayerState
    {
        public DeadState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            MusicManager.Instance.SetMusic(4);
            //OnPyrDeath?.Invoke();
            // Temporal Feedback Queue
            //_PM.m_sprite.color = Color.black;
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
    private class StunnedState : PlayerState
    {
        private float m_StunDuration = 2f; // Duration of the stun in seconds
        public StunnedState(C_PlayerMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
    private class GrabedState : PlayerState
    {
        private float m_GrabDamage;
        private int m_CurMashCount;
        private C_MonsterMotor m_MonsterThatGrabbed;
        private Vector2 m_PlayerGrabbedPos;
        private bool m_CanMashOutOfGrab;

        public GrabedState(C_PlayerMotor motor, C_MonsterMotor monsterThatGrabbed, Vector2 playerGrabbedPos, 
            bool canMashOutOfGrab) : base(motor) {
            m_MonsterThatGrabbed = monsterThatGrabbed;
            m_PlayerGrabbedPos = playerGrabbedPos;
            m_CanMashOutOfGrab = canMashOutOfGrab;
        }
        public override void MyEnter()
        {
            base.MyEnter();
            // Temporal Feedback Queue
            //_PM.m_sprite.color = Color.red;
            m_CurMashCount = _PM.m_GrabMashCount;
            if (m_MonsterThatGrabbed is C_Monst_Tree)
            {
                // Do something specific for C_Monst_Tree
                //m_GrabDamage = (_PM.m_MonsterThatGrabbed as C_Monst_Tree).GrabDamageAmountPerInterval;
                m_GrabDamage = (m_MonsterThatGrabbed as C_Monst_Tree).GrabDamageAmountPerInterval;
            }
        }
        public override void MyUpdate()
        {
            base.MyUpdate();

            _PM.PlayerStats.RecieveDamage 
                (m_MonsterThatGrabbed.gameObject.GetComponent<Collider2D>()
                ,(m_MonsterThatGrabbed as C_Monst_Tree).GrabDamageAmountPerInterval
                ,(m_MonsterThatGrabbed as C_Monst_Tree).GrabDamageIntervalTime,
                (m_MonsterThatGrabbed as C_Monst_Tree).GrabFearAmountPerInterval);
            _PM.CheckIfDeath();
            //_PM.gameObject.transform.position = _PM.m_MonsterThatGrabbed.PlayerGrabbedPosition + (Vector2)_PM.m_MonsterThatGrabbed.transform.position;
            _PM.transform.position = m_PlayerGrabbedPos + (Vector2)m_MonsterThatGrabbed.transform.position;
            if (m_CanMashOutOfGrab)
            {
                KeyMeshingMinigame();
            }
        }
        private void KeyMeshingMinigame()
        {
            Debug.Log("Cantidad de clicks necesarias: " + m_CurMashCount);
            if (PlayerInputs.Instance.InteractAndPickUpItemBool)
            {
                m_CurMashCount--;
                Debug.Log("Player hizo mash! " + m_CurMashCount);
                if (m_CurMashCount <= 0)
                {
                    Debug.Log("Player se liberó del agarre completamente!");
                    //_PM.m_MonsterThatGrabbed.ReleaseGrab();

                    //_PM.m_MonsterThatGrabbed.PlayerAction1_Bool = true;
                    // DEBE DE HABER ALGUNA MEJOR MANERA PARA COMUNICARSE CON EL MONSTRUO QUE AGARRÓ AL JUGADOR, PERO POR AHORA ESTO FUNCIONA
                    var Tree = m_MonsterThatGrabbed as C_Monst_Tree;
                    var TreeVar = Tree.m_CurrentState as C_Monst_Tree.S_Tree_Grab;
                    TreeVar.ReleasePlayer = true;

                    _PM.ChangeState(new RegularState(_PM));
                }
            }
        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
}

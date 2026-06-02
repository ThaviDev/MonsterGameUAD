using UnityEditor.XR;
using UnityEngine;

public class C_Monst_Tree : C_MonsterMotor
{
    [Header("Monster Tree Settings")]

    [Header("Abilities")]
    [SerializeField] private float m_GrabDamageIntervalTime;
    public float GrabDamageIntervalTime { get { return m_GrabDamageIntervalTime; } }
    [SerializeField] private float m_GrabDamageAmountPerInterval;
    public float GrabDamageAmountPerInterval { get { return m_GrabDamageAmountPerInterval; } }
    [SerializeField] private Vector2 m_ColOffset = new Vector2(-1, 0);
    [SerializeField] private Vector2 m_ColSize = new Vector2(2, 2);
    [SerializeField] private Vector2 m_PlayerGrabbedPosition;

    [SerializeField] private Vector2 m_GroundPoundRadius;
    [SerializeField] private float m_GroundPoundDamageAmount;

    [SerializeField] private Collider2D m_ChargeHitbox;
    [SerializeField] private float m_ChargeSpeed;
    [SerializeField] private float m_ChargeDuration;
    [SerializeField] private float m_ChargeDamageAmount;
    [SerializeField] private float m_ChargeKnockbackForce;
    [SerializeField] private float m_ChargeRecoveryTime;

    [SerializeField] private C_MAnim_Tree m_AnimTree;
    [SerializeField] private bool m_ReleasePlayer;
    public bool ReleasePlayer { get { return m_ReleasePlayer; } set { m_ReleasePlayer = value; } }

    protected override void Start()
    {
        base.Start();
        m_AnimTree = m_Visual as C_MAnim_Tree;
    }
    protected override void Update()
    {
        base.Update();

    }
    private void CheckIfSlam()
    {

    }
    protected override void OnTriggerEnter2D(Collider2D otherCol)
    {
        base.OnTriggerEnter2D(otherCol);

        // -- TRIGGER GRAB --
        if (((1 << otherCol.gameObject.layer) & (int)m_PlayerLayerMask) != 0 && m_CurrentState is not S_Tree_Grab)
        {
            ChangeState(new S_Tree_Grab(this));
        }
    }
    public class S_Tree_Grab : C_MonstState
    {
        private GameObject m_GrabbedPlayer;
        private C_Monst_Tree m_Tree;
        private C_MAnim_Tree m_TreeAnim;
        private bool m_ReleasePlayer;
        public bool ReleasePlayer { get { return m_ReleasePlayer; } set { m_ReleasePlayer = value; } }
        public S_Tree_Grab(C_Monst_Tree motor) : base(motor)
        {
            m_Tree = motor as C_Monst_Tree;
            m_TreeAnim = m_Tree.Visual as C_MAnim_Tree;
            if (m_Tree == null)
                Debug.LogError("S_Tree_Grab solo puede ser usado por C_Monst_Tree");
        }
        public override void MyEnter()
        {
            base.MyEnter();
            m_TreeAnim.AnimGrab();
            //m_Tree.Visual.AnimAbility();
            //print("Estoy Iniciando Agarre");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            _monst.DecreaseEnergy();
            _monst.DecreaseAgression();
            if (m_TreeAnim.GrabPlayer)
            {
                TryGrab();
            }
            if (m_Tree.m_ReleasePlayer)
            {
                m_Tree.ChangeState(new S_Spawned(m_Tree));
            }
        }
        public void TryGrab()
        {
            Debug.Log("Intento Agarrar");
            Vector2 center = (Vector2)m_Tree.transform.position + m_Tree.m_ColOffset;
            Collider2D hit = Physics2D.OverlapBox(center, m_Tree.m_ColSize, 0f, m_Tree.PlayerLayerMask);
            if (hit != null)
            {
                print("Grabbed player: " + hit.gameObject.name);
                if (m_TreeAnim != null)
                    m_TreeAnim.IsPlayerGrabed = true;
                m_GrabbedPlayer = hit.gameObject;
                m_GrabbedPlayer.GetComponent<C_PlayerMotor>()?.GetGrabbed(m_Tree, m_Tree.m_PlayerGrabbedPosition, true);
            }
            else
            {
                if (m_TreeAnim != null)
                    m_TreeAnim.IsPlayerGrabed = false;
                //m_TreeAnim.AnimGrabRelease();
                m_Tree.ChangeState(new S_Spawned(m_Tree));
            }
        }
        /*
        private void ApplyGrabDamage()
        {
            if (m_GrabbedPlayer != null)
            {
                var playerStats = m_GrabbedPlayer.GetComponent<C_PlayerStats>();
                var playerMotor = m_GrabbedPlayer.GetComponent<C_PlayerMotor>();
                // Aplicar daño al jugador agarrado
                playerStats.RecieveDamage(_monst.gameObject.transform.GetComponent<Collider2D>(), m_Tree.m_GrabDamageAmountPerInterval);
                //m_GrabbedPlayer.transform.position = _monst.transform.position; // Mantener al jugador en la posición del árbol
                //_monst.m_GrabbedPlayer.GetComponent<Player>();
            }
        }*/
        public override void MyExit()
        {
            base.MyExit();
            m_TreeAnim.AnimGrabRelease();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
        }
    }
    public class S_Tree_GroundPound : C_MonstState
    {
        private C_Monst_Tree m_Tree;
        private C_MAnim_Tree m_TreeAnim;
        public S_Tree_GroundPound(C_MonsterMotor motor) : base(motor) {
            m_Tree = motor as C_Monst_Tree;
            m_TreeAnim = m_Tree.Visual as C_MAnim_Tree;
            if (m_Tree == null)
                Debug.LogError("S_Tree_Grab solo puede ser usado por C_Monst_Tree");
        }
        public override void MyEnter()
        {
            base.MyEnter();
            //_monst.Visual.AnimAbility();
            //print("Estoy Iniciando Ground Pound");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            _monst.DecreaseEnergy();
            _monst.DecreaseAgression();
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
    public class S_Tree_Charge : C_MonstState
    {
        public S_Tree_Charge(C_MonsterMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            //_monst.Visual.AnimAbility();
            //print("Estoy Iniciando Charge");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            _monst.DecreaseEnergy();
            _monst.DecreaseAgression();
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

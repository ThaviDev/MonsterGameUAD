using UnityEngine;

public class C_Monst_Tree : C_MonsterMotor
{
    [Header("Monster Tree Settings")]

    [Header("Abilities")]
    [SerializeField] private float m_GrabDamageIntervalTime;
    [SerializeField] private float m_GrabDamageAmountPerInterval;

    [SerializeField] private float m_GroundPoundRadius;
    [SerializeField] private float m_GroundPoundDamageAmount;

    [SerializeField] private float m_ChargeSpeed;
    [SerializeField] private float m_ChargeDuration;
    [SerializeField] private float m_ChargeDamageAmount;
    [SerializeField] private float m_ChargeKnockbackForce;
    [SerializeField] private float m_ChargeRecoveryTime;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeState(new S_Tree_Grab(this));
        }
    }
    protected override void OnTriggerEnter2D(Collider2D otherCol)
    {
        base.OnTriggerEnter2D(otherCol);
    }
    public class S_Tree_Grab : C_MonstState
    {
        private bool m_IsPlayerGrabbed;
        private GameObject m_GrabbedPlayer;
        private C_Monst_Tree m_Tree;
        public S_Tree_Grab(C_Monst_Tree motor) : base(motor) { 
            m_Tree = motor as C_Monst_Tree;
            if (m_Tree == null)
                Debug.LogError("S_Tree_Grab solo puede ser usado por C_Monst_Tree");
        }
        public override void MyEnter()
        {
            base.MyEnter();
            m_Tree.Visual.AnimAbility();
            //print("Estoy Iniciando Agarre");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            _monst.DecreaseEnergy();
            _monst.DecreaseAgression();
        }
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

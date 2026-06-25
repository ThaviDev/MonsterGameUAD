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
    [SerializeField] private float m_GrabFearAmountPerInterval;
    public float GrabFearAmountPerInterval { get { return m_GrabFearAmountPerInterval; } }
    [SerializeField] private BoxCollider2D m_GrabHitbox;
    [SerializeField] private Vector2 m_ColOffset = new Vector2(-1, 0);
    [SerializeField] private Vector2 m_ColSize = new Vector2(2, 2);
    [SerializeField] private Vector2 m_PlayerGrabbedPositionLeft;
    [SerializeField] private Vector2 m_PlayerGrabbedPositionRight;

    [SerializeField] private float m_GroundPoundRadius;
    [SerializeField] private float m_GroundPoundDamageAmount;

    [SerializeField] private Collider2D m_ChargeHitbox;
    [SerializeField] private float m_ChargeSpeed;
    [SerializeField] private float m_ChargeDuration;
    [SerializeField] private float m_ChargeDamageAmount;
    [SerializeField] private float m_ChargeKnockbackForce;
    [SerializeField] private float m_ChargeRecoveryTime;

    [SerializeField] private C_MAnim_Tree m_AnimTree;

    protected override void Start()
    {
        base.Start();
        m_AnimTree = m_Visual as C_MAnim_Tree;
    }
    protected override void Update()
    {
        base.Update();
        m_GrabHitbox.size = m_ColSize;
        m_GrabHitbox.offset = m_ColOffset;
        Debug.Log("Tree Current State: " + m_CurrentState);
    }
    public override void Despawn()
    {
        RandomizeSpawnAndDespawnValues();
        ChangeState(new S_Tree_Despawned(this));
    }
    public override void Spawn()
    {
        RandomizeSpawnAndDespawnValues();
        ChangeState(new S_Tree_Spawned(this));
    }
    protected override void OnTriggerEnter2D(Collider2D otherCol)
    {
        base.OnTriggerEnter2D(otherCol);
    }
    public class S_Tree_Despawned : S_Despawned
    {
        public S_Tree_Despawned(C_MonsterMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            //Debug.Log("Estoy Despawneado y ademas soy arbol");
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
    public class S_Tree_Spawned : S_Spawned
    {
        public S_Tree_Spawned(C_MonsterMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            //Debug.Log("Estoy Spawneado y ademas soy arbol");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void StablishState()
        {
            if (Motor.Agresion >= Motor.AgressionChaseThreshold)
            {
                Motor.ChangeState(new S_Tree_Chasing(Motor));
            }
            else
            {
                Motor.ChangeState(new S_Tree_Stealthy(Motor));
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
    public class S_Tree_Chasing : S_Chasing
    {
        private C_Monst_Tree m_Tree;
        private float m_DistanceToPlayer;
        public S_Tree_Chasing(C_MonsterMotor motor) : base(motor) {
            m_Tree = motor as C_Monst_Tree;
        }
        public override void MyEnter()
        {
            base.MyEnter();
            //m_Tree = Motor as C_Monst_Tree;
            //Debug.Log("Estoy persiguiendo y ademas soy arbol");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            m_DistanceToPlayer = Vector2.Distance(Motor.transform.position, Motor.PlayerMotor.transform.position);

            CheckSlam();
            CheckCharge();

            if (m_Tree.m_IsTurnedLeft == false)
            {
                m_Tree.m_ColOffset.x = -1;
            }
            else
            {
                m_Tree.m_ColOffset.x = 1;
            }
        }

        public void CheckSlam()
        {
            if (m_DistanceToPlayer <= m_Tree.m_GroundPoundRadius)
            {
                m_Tree.ChangeState(new S_Tree_GroundPound(m_Tree));
            }
        }
        public void CheckCharge()
        {

        }
        public override void MyExit()
        {
            base.MyExit();
        }
        public override void MyTriggerColision(Collider2D other)
        {
            base.MyTriggerColision(other);
            
            // -- TRIGGER GRAB --
            if (((1 << other.gameObject.layer) & (int)Motor.PlayerLayerMask) != 0 && Motor.m_CurrentState is not S_Tree_Grab)
            {
                Motor.ChangeState(new S_Tree_Grab(Motor as C_Monst_Tree));
            }

        }
    }
    public class S_Tree_Stealthy : S_Stealthy
    {
        public S_Tree_Stealthy(C_MonsterMotor motor) : base(motor) { }
        public override void MyEnter()
        {
            base.MyEnter();
            //Debug.Log("Estoy sigiloso y ademas soy arbol");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
        }
        public override void PlayerHasSeenMe()
        {
            //base.PlayerHasSeenMe(newState);
            if (Motor.HasBeenFoundByPlayer)
            {
                Motor.DecreasingEnergyWhenFound();
                Motor.Despawn();
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
    public class S_Tree_Grab : C_MonstState
    {
        private GameObject m_GrabbedPlayer;
        private C_Monst_Tree m_Tree;
        private C_MAnim_Tree m_TreeAnim;
        private bool m_GrabTrigger;
        private bool m_ReleasePlayer;
        // Variable para comunicarse con C_PlayerMotor y liberar al jugador desde ahí
        public bool ReleasePlayer { get { return m_ReleasePlayer; } set { m_ReleasePlayer = value; } }
        public S_Tree_Grab(C_MonsterMotor motor) : base(motor)
        {
            m_Tree = motor as C_Monst_Tree;
            m_TreeAnim = m_Tree.Visual as C_MAnim_Tree;
            if (m_Tree == null)
                Debug.LogError("S_Tree_Grab solo puede ser usado por C_Monst_Tree");
        }
        public override void MyEnter()
        {
            base.MyEnter();
            // Llamar animación de agarre
            m_TreeAnim.AnimGrab();
            m_GrabTrigger = true;
            m_Tree.Boid.StopMovementTime = 999f;
        }
        public override void MyUpdate()
        {
            print("Puedo liberar a jugaror: " + m_ReleasePlayer);
            base.MyUpdate();
            Motor.DecreaseEnergy();
            Motor.DecreaseAgression();
            if (m_TreeAnim.GrabPlayer && m_GrabTrigger)
            {
                TryGrab();
            }
            if (m_ReleasePlayer)
            {
                ReturnToChaseState();
            }
        }

        private void ReturnToChaseState()
        {
            if (m_TreeAnim != null)
            {
                m_TreeAnim.GrabPlayer = false;
                m_TreeAnim.IsPlayerGrabed = false;
            }
            m_Tree.Boid.StopMovementTime = 0.25f;
            m_Tree.ChangeState(new S_Tree_Chasing(m_Tree));
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
                // Checa si esta izquierda o derecha(IF-THEN-ELSE) Este-> ? : 
                C_PlayerMotor.OnGetGrabbed?.Invoke
                    (m_Tree,
                    m_Tree.m_IsTurnedLeft ? 
                    m_Tree.m_PlayerGrabbedPositionLeft : 
                    m_Tree.m_PlayerGrabbedPositionRight
                    , true);
                //m_GrabbedPlayer.GetComponent<C_PlayerMotor>()?.GetGrabbed(m_Tree, m_Tree.m_PlayerGrabbedPosition, true);
            }
            else {
                ReturnToChaseState();
            }
            m_GrabTrigger = false;
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
                Debug.LogError("S_Tree_GroundPound solo puede ser usado por C_Monst_Tree");
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
            Motor.DecreaseEnergy();
            Motor.DecreaseAgression();
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
        private C_Monst_Tree m_Tree;
        private C_MAnim_Tree m_TreeAnim;
        public S_Tree_Charge(C_MonsterMotor motor) : base(motor) {
            m_Tree = motor as C_Monst_Tree;
            m_TreeAnim = m_Tree.Visual as C_MAnim_Tree;
            if (m_Tree == null)
                Debug.LogError("S_Tree_Charge solo puede ser usado por C_Monst_Tree");
        }
        public override void MyEnter()
        {
            base.MyEnter();
            //_monst.Visual.AnimAbility();
            //print("Estoy Iniciando Charge");
        }
        public override void MyUpdate()
        {
            base.MyUpdate();
            Motor.DecreaseEnergy();
            Motor.DecreaseAgression();
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

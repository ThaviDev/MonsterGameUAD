using SteeringBehaviours;
using UnityEngine;

public class C_Monster : MonoBehaviour
{
    // Script Viejo, se puede usar como referencia para el nuevo script de C_MonsterMotor, pero no se va a usar directamente.
    [Header("Monster Settings")]
    protected GameObject m_PlayerObjRef;
    [SerializeField] protected C_Boid m_Boid;
    [SerializeField] protected float m_Speed;
    [SerializeField] protected SpriteRenderer m_VisualSpr;
    [SerializeField] protected Animator m_VisualAnim;

    [SerializeField] protected bool m_IsSpawnedIn;
    [SerializeField] protected bool m_IsAggressive;

    [SerializeField] private float m_EnergyRegenRate;
    [SerializeField] private float m_EnergyDepleationRate;
    [SerializeField] private float m_StartEnergy;
    [SerializeField] protected float m_Energy;
    private float m_MaxEnergy;
    // The minimum amount of energy the monster needs to spawn
    [SerializeField] private float m_MinEnergyToSpawn;
    // The specific amount of energy for the monster to spawn
    [SerializeField] private float m_EnergySetToSpawn;
    // The maximum amount of energy the monster identifies to despawn
    [SerializeField] private float m_MaxEnergyToDespawn;
    // The specific amount of energy for the monster to despawn
    [SerializeField] private float m_EnergySetToDespawn;
    [SerializeField] private float m_EnergyRandomScaleSpawn;
    [SerializeField] private float m_EnergyRandomScaleDespawn;

    [SerializeField] private float m_GrabDamage;
    public float GrabDamage { get { return m_GrabDamage; } }
    [SerializeField] private float m_GroundHitDamage;
    public float GroundHitDamage { get { return m_GroundHitDamage; } }
    [SerializeField] private float m_ChargeDamage;
    public float ChargeDamage { get { return m_ChargeDamage; } }
    // Player Action 1 es: Cualquier accion que el jugador toma para contrarestar alguna habilidad del monstruo
    /* Arbol: Mash out para liberarse del agarre
     * 
     */
    protected bool m_PlayerAction1;
    public bool PlayerAction1_Bool { get { return m_PlayerAction1; } set { m_PlayerAction1 = value; } }

    // Generic state machine for base-level states (spawn/despawn)
    //protected StateMachine<C_Monster> m_StateMachine;
    protected MonsterState m_CurrentState;

    protected virtual void Start()
    {
        if (m_Boid == null)
        {
            m_Boid = gameObject.GetComponent<C_Boid>();
            if (m_Boid == null)
            {
                Debug.LogError("C_Monster: No Boid component found on " + gameObject.name);
            }
        }
        m_Boid.BoidMaxSpeed = m_Speed;

        m_Energy = m_StartEnergy;
        RandomizeSpawnAndDespawnValues();

        m_IsSpawnedIn = true;
        m_VisualAnim.SetBool("SpawnedIn", true);
        m_MaxEnergy = 10000;
    }
    protected virtual void Update()
    {
        // El codigo de aqui aplicara sin acceder a nada de otro monstruo
        Energy();
        m_CurrentState?.MyUpdate();
    }

    protected virtual void RandomizeSpawnAndDespawnValues()
    {
        m_EnergySetToSpawn = Random.Range(m_MinEnergyToSpawn, m_MinEnergyToSpawn + m_EnergyRandomScaleSpawn);
        m_EnergySetToDespawn = Random.Range(m_MaxEnergyToDespawn - m_EnergyRandomScaleDespawn, m_MaxEnergyToDespawn);
    }

    protected virtual void Energy()
    {
        if (m_IsSpawnedIn)
        {
            print("Checo Si accedo a mi spawneamiento");
            if (m_Energy > m_MaxEnergy)
            {
                m_Energy = m_MaxEnergy;
            }
            else
            {
                print("Reduzco mi energia");
                m_Energy -= m_EnergyDepleationRate * Time.deltaTime;
            }
            if (m_Energy <= m_EnergySetToDespawn)
            {
                print("Despawnear");
                BegginDespawning();
            }
        }
        else
        {
            m_Energy += m_EnergyRegenRate * Time.deltaTime;
            if (m_Energy >= m_EnergySetToSpawn)
            {
                print("Spawnear");
                BegginSpawning();
            }
        }
    }
    protected virtual void BegginSpawning()
    {
        // Empezar animacion de spawn
        m_VisualAnim.SetBool("SpawnedIn", true);
        m_IsSpawnedIn = true;
        RandomizeSpawnAndDespawnValues();
        // Aquí sería mejor tener una animacion de spawn, pero por ahora se hace inmediatamente
        Spawn();
    }
    protected virtual void BegginDespawning()
    {
        m_VisualAnim.SetBool("SpawnedIn", false);
        m_IsSpawnedIn = false;
        RandomizeSpawnAndDespawnValues();
        // Aquí sería mejor tener una animacion de despawn, pero por ahora se hace inmediatamente
        Despawn();
    }
    // Metodo llamado por la ANIMACION
    public virtual void Spawn()
    {
        m_IsSpawnedIn = true;
        ChangeState(new SpawnedState(this));
    }
    // Metodo llamado por la ANIMACION
    public virtual void Despawn()
    {
        m_IsSpawnedIn = false;
        ChangeState(new DespawnedState(this));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
         m_CurrentState?.MyTriggerColision(col);
    }

    // Base spawn/despawn states implemented using generic State<T>
    public abstract class MonsterState
    {
        protected C_Monster _M;
        public MonsterState(C_Monster monster)
        {
            _M = monster;
        }
        public virtual void MyEnter() { }
        public virtual void MyUpdate() { }
        public virtual void MyExit() { }
        public virtual void MyTriggerColision(Collider2D col) { }
    }
    protected void ChangeState(MonsterState newState)
    {
        if (newState == null)
        {
            return;
        }
        m_CurrentState?.MyExit();
        m_CurrentState = newState;
        m_CurrentState.MyEnter();
    }
    protected class DespawnedState : MonsterState
    {
        public DespawnedState(C_Monster Monster) : base(Monster) { }
        public override void MyEnter()
        {
            _M.m_VisualSpr.enabled = false;
            _M.m_Boid.BoidMaxSpeed = 0;
            print("Despawneo Default");
        }
        public override void MyUpdate()
        {

        }
        public override void MyExit()
        {
            _M.m_VisualSpr.enabled = true;
        }
    }
    protected class SpawnedState : MonsterState
    {
        public SpawnedState(C_Monster Monster) : base(Monster) { }
        public override void MyEnter()
        {
            _M.m_Boid.BoidMaxSpeed = _M.m_Speed;
        }
        public override void MyUpdate()
        {

        }
        public override void MyExit()
        {

        }
    }
}

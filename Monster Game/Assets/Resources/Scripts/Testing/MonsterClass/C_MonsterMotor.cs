using SteeringBehaviours;
using UnityEngine;

public class C_MonsterMotor : MonoBehaviour
{
    [Header("References")]
    protected GameObject m_PlayerObjRef;
    [SerializeField] protected C_Boid m_Boid;
    public C_Boid Boid { get { return m_Boid; } }
    [SerializeField] protected C_MonsAnimBase m_Visual;
    public C_MonsAnimBase Visual { get { return m_Visual; } }
    [SerializeField] protected C_PlayerMotor m_PlayerMotor;
    public C_PlayerMotor PlayerMotor { get { return m_PlayerMotor; } }
    [SerializeField] protected Transform m_PredictionPoint;
    public Transform PredictionPoint { get { return m_PredictionPoint; } }
    //[SerializeField] protected SpriteRenderer m_VisualSpr;
    //[SerializeField] protected Animator m_VisualAnim;

    [Header("Stats")]
    [SerializeField] protected float m_Energy = 0;
    [SerializeField] protected float m_EnergyMax = 1000;
    [SerializeField] protected float m_EnergyMin = 0;
    [SerializeField] protected float m_EnergyRegenIncrement = 1.25f;
    [SerializeField] protected float m_EnergySpawnedDecrement = 0.75f;

    // The minimum amount of energy the monster needs to spawn
    [SerializeField] protected float m_EnergyMinToSpawn = 500f;
    // The specific amount of energy for the monster to spawn
    [SerializeField] protected float m_EnergySetToSpawn = 750f;
    [SerializeField] protected float m_EnergyRandomScaleSpawn;
    [SerializeField] protected float m_MinimumTimeDespawned = 3f;

    // The minimum amount of energy the monster identifies to despawn
    [SerializeField] protected float m_EnergyMaxToDespawn = 200f;
    // The specific amount of energy for the monster to despawn
    [SerializeField] protected float m_EnergySetToDespawn = 100f;
    [SerializeField] protected float m_EnergyRandomScaleDespawn;

    [SerializeField] protected float m_Agression = 0;
    public float Agresion { get { return m_Agression; } }
    [SerializeField] protected float m_AgressionMax = 1000;
    [SerializeField] protected float m_AgressionRegenIncrement = 0.5f;
    [SerializeField] protected float m_AgressionChaseDecrement = 3f;
    [SerializeField] protected float m_AgressionChaseThreshold = 500f;
    public float AgressionChaseThreshold { get { return m_AgressionChaseThreshold; } }

    [SerializeField] protected float m_SpeedCur = 1;
    public float SpeedCur { get { return m_SpeedCur; } }
    [SerializeField] protected float m_SpeedMax = 5;

    [Header("Settings")]

    [SerializeField] protected bool m_IsSpawnedIn;

    [SerializeField] protected Vector3 m_StealthPoint;

    protected C_MonstState m_CurrentState;
    protected virtual void Start()
    {
        m_Visual = gameObject.transform.GetChild(0).gameObject.transform.GetComponent<C_MonsAnimBase>();
        m_Boid = GetComponent<C_Boid>();
        m_PlayerMotor = FindFirstObjectByType<C_PlayerMotor>();
        m_PredictionPoint = FindFirstObjectByType<C_PlayerPredictionPoint>().transform;
        RandomizeSpawnAndDespawnValues();
        ChangeState(new S_Despawned(this));
    }

    protected virtual void Update()
    {
        m_CurrentState?.MyUpdate();
    }

    public virtual void RegenEnergy() {

        m_Energy += m_EnergyRegenIncrement * Time.deltaTime;

        if (m_Energy > m_EnergyMax)
        {
            m_Energy = m_EnergyMax;
        }

        if (m_Energy >= m_EnergySetToSpawn)
        {
            print("Spawnear");
            CallSpawn();
        }
    }
    public virtual void DecreaseEnergy() {

        m_Energy -= m_EnergySpawnedDecrement * Time.deltaTime;

        if (m_Energy < m_EnergyMin)
        {
            m_Energy = m_EnergyMin;
        }

        if (m_Energy <= m_EnergySetToDespawn)
        {
            print("Despawnear");
            CallDespawn();
        }
    }
    public virtual void RegenAgression() {
        m_Agression += m_AgressionRegenIncrement * Time.deltaTime;

        if (m_Agression > m_AgressionMax)
        {
            m_Agression = m_AgressionMax;
        }
    }
    public virtual void DecreaseAgression() {
        m_Agression -= m_AgressionChaseDecrement * Time.deltaTime;

        if (m_Agression < 0)
        {
            m_Agression = 0;
        }
    }

    protected virtual void CallSpawn()
    {
        // Empezar animacion de spawn
        //m_VisualAnim.SetBool("SpawnedIn", true);
        //m_Visual.AnimSpawn();
        m_IsSpawnedIn = true;
        RandomizeSpawnAndDespawnValues();
        // Aquí sería mejor tener una animacion de spawn, pero por ahora se hace inmediatamente
        Spawn();
    }
    protected virtual void CallDespawn()
    {
        //m_VisualAnim.SetBool("SpawnedIn", false);
        //m_Visual.AnimDespawn();
        m_IsSpawnedIn = false;
        RandomizeSpawnAndDespawnValues();
        // Aquí sería mejor tener una animacion de despawn, pero por ahora se hace inmediatamente
        Despawn();
    }
    protected virtual void Spawn()
    {
        m_IsSpawnedIn = true;
        ChangeState(new S_Spawned(this));
    }
    protected virtual void Despawn()
    {
        m_IsSpawnedIn = false;
        ChangeState(new S_Despawned(this));
    }
    protected virtual void RandomizeSpawnAndDespawnValues()
    {
        m_EnergySetToSpawn = Random.Range(m_EnergyMinToSpawn, m_EnergyMinToSpawn + m_EnergyRandomScaleSpawn);
        m_EnergySetToDespawn = Random.Range(m_EnergyMaxToDespawn - m_EnergyRandomScaleDespawn, m_EnergyMaxToDespawn);
    }
    public virtual void ChangeState(C_MonstState newState)
    {
        if (newState == null)
        {
            return;
        }
        m_CurrentState?.MyExit();
        m_CurrentState = newState;
        m_CurrentState.MyEnter();
    }
    protected virtual void OnTriggerEnter2D(Collider2D otherCol)
    {
        m_CurrentState?.MyTriggerColision(otherCol);
    }
}

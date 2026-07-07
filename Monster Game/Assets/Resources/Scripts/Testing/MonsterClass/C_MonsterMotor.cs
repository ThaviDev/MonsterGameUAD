using SteeringBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MonsterMotor : MonoBehaviour
{
    [Header("References")]
    // Expose layer mask so it's editable in Inspector; fallback to "Player" in Start.
    [SerializeField] protected LayerMask m_PlayerLayerMask;
    public LayerMask PlayerLayerMask { get { return m_PlayerLayerMask; } }
    protected GameObject m_PlayerObjRef;
    [SerializeField] protected C_Boid m_Boid;
    public C_Boid Boid { get { return m_Boid; } }
    [SerializeField] protected C_MonsAnimBase m_Visual;
    public C_MonsAnimBase Visual { get { return m_Visual; } }
    [SerializeField] protected C_PlayerMotor m_PlayerMotor;
    public C_PlayerMotor PlayerMotor { get { return m_PlayerMotor; } }
    [SerializeField] protected Transform m_PredictionPoint;
    public Transform PredictionPoint { get { return m_PredictionPoint; } }
    [SerializeField] protected C_AStar m_PathFinder;
    public C_AStar PathFinder { get { return m_PathFinder; } }
    [SerializeField] protected AudioSource m_AudioSource;
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

    [Header("General Variables")]
    [SerializeField] protected float m_SpeedCur = 1;
    public float SpeedCur { get { return m_SpeedCur; } }
    [SerializeField] protected float m_SpeedMax = 5;
    [SerializeField] protected bool m_HasBeenFoundByPlayer;
    public bool HasBeenFoundByPlayer { get { return m_HasBeenFoundByPlayer; } set { m_HasBeenFoundByPlayer = value; } }
    [SerializeField] protected float m_EnergyDecreasedWhenFound;
    [SerializeField] protected float m_EnergyIncreasedWhenScreamer;
    [SerializeField] protected bool m_IsMoving;

    [Header("Settings")]

    //[SerializeField] protected bool m_IsSpawnedIn;

    [SerializeField] protected Vector3 m_StealthPoint;

    public C_MonstState m_CurrentState;

    protected List<Vector3> m_ChasePath;

    protected bool m_IsTurnedLeft;
    protected virtual void Start()
    {
        m_Visual = gameObject.transform.GetChild(0).gameObject.transform.GetComponent<C_MonsAnimBase>();
        m_Boid = GetComponent<C_Boid>();
        m_PlayerMotor = FindFirstObjectByType<C_PlayerMotor>();
        m_PredictionPoint = FindFirstObjectByType<C_PlayerPredictionPoint>().transform;
        m_PathFinder = FindFirstObjectByType<C_AStar>();
        m_AudioSource = GetComponent<AudioSource>();
        RandomizeSpawnAndDespawnValues();
        Despawn();
        //ChangeState(new S_Despawned(this));
    }
    protected virtual void Update()
    {
        m_CurrentState?.MyUpdate();

        if (m_Boid.BoidMoveForce.x > 0)
        {
            m_IsTurnedLeft = true;
        }
        else if (m_Boid.BoidMoveForce.x < 0)
        {
            m_IsTurnedLeft = false;
        }
        // IsTurnedLeft tambien es usado por el agarre para saber a donde ve el monstruo
        m_Visual.GetComponent<SpriteRenderer>().flipX = m_IsTurnedLeft;
    }
    public virtual void StartPathUpdater()
    {
        m_IsMoving = true;
        StartCoroutine(UpdatePath());
        Debug.Log("Empezar a seguir al jugador");
    }
    public virtual void StopPathUpdater()
    {
        m_IsMoving = false;
        m_ChasePath = null;
        Boid.Path = null;
        StopCoroutine(UpdatePath());
        Debug.Log("Dejar de seguir al jugador");
    }
    public virtual IEnumerator UpdatePath()
    {
        while (m_IsMoving)
        {
            m_ChasePath = PathFinder.GetPath(gameObject.transform.position, PredictionPoint.position);
            Boid.Path = m_ChasePath;
            yield return new WaitForSeconds(0.2f); // Actualiza el camino cada 0.5 segundos
        }
    }

    public virtual void RegenEnergy() {

        m_Energy += m_EnergyRegenIncrement * Time.deltaTime;

        if (m_Energy > m_EnergyMax)
        {
            m_Energy = m_EnergyMax;
        }

        if (m_Energy >= m_EnergySetToSpawn)
        {
            //print("Spawnear");
            Spawn();
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
            //print("Despawnear");
            Despawn();
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
    public virtual void Spawn()
    {
        //m_IsSpawnedIn = true;
        RandomizeSpawnAndDespawnValues();
        ChangeState(new S_Spawned(this));
    }
    public virtual void Despawn()
    {
        //m_IsSpawnedIn = false;
        RandomizeSpawnAndDespawnValues();
        ChangeState(new S_Despawned(this));
    }
    public virtual void RandomizeSpawnAndDespawnValues()
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
    public virtual void DecreasingEnergyWhenFound()
    {
        m_Energy -= m_EnergyDecreasedWhenFound;
    }
    public virtual void IncreasingEneryWhenScreamer()
    {
        m_Energy += m_EnergyIncreasedWhenScreamer;
    }
    protected virtual void OnTriggerEnter2D(Collider2D otherCol)
    {
        m_CurrentState?.MyTriggerColision(otherCol);
    }
    protected virtual void OnCollisionEnter2D(Collision2D otherCol)
    {
        print("Choque con algo: " + otherCol.gameObject.layer + " " + otherCol);
        m_CurrentState?.MyColision(otherCol);
    }
}
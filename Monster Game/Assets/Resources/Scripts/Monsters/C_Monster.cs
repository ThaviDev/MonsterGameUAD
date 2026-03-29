using UnityEngine;

public class C_Monster : MonoBehaviour
{
    [Header("Monster Settings")]
    protected GameObject m_PlayerObjRef;
    [SerializeField] protected bool m_IsSpawnedIn;
    [SerializeField] protected bool m_IsAggressive;
    [SerializeField] protected float m_Speed;
    [SerializeField] private float m_EnergyRegenRate;
    [SerializeField] private float m_EnergyDepleationRate;
    [SerializeField] private float m_StartEnergy;
    private float m_Energy;
    private float m_MaxEnergy;
    // The minimum amount of energy the monster needs to spawn
    private float m_MinEnergyToSpawn;
    // The specific amount of energy for the monster to spawn
    private float m_EnergySetToSpawn;
    // The maximum amount of energy the monster identifies to despawn
    private float m_MaxEnergyToDespawn;
    // The specific amount of energy for the monster to despawn
    private float m_EnergySetToDespawn;
    
    // Player Action 1 es: Cualquier accion que el jugador toma para contrarestar alguna habilidad del monstruo
    /* Arbol: Mash out para liberarse del agarre
     * 
     */
    protected bool m_PlayerAction1;
    public bool PlayerAction1_Bool { get { return m_PlayerAction1; } set { m_PlayerAction1 = value; } }


    protected virtual void Start()
    {
        // El codigo de aqui aplicara sin acceder a nada de otro monstruo
    }
    protected virtual void Update()
    {
        print("Accedo a Update");
        // El codigo de aqui aplicara sin acceder a nada de otro monstruo
    }
    public virtual void Spawn()
    {

    }
    public virtual void Despawn()
    {

    }
    public virtual void ForcePlayerPosition(GameObject player)
    {

    }
    public virtual void ReleasePlayerPosition(GameObject player) 
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MyColisionTrigger(collision);
    }
    public virtual void MyColisionTrigger(Collider2D col)
    {

    }
}

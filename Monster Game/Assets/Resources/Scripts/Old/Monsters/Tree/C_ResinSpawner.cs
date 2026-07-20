using UnityEngine;

public class C_ResinSpawner : MonoBehaviour
{
    // Probablemente se deberia de crear una clase global para spawner constante
    public GameObject m_Prefab;
    [SerializeField] private float m_PeriodicTiming = 1;
    private float m_Time;
    void Start()
    {
        m_Time = m_PeriodicTiming;
    }
    void Update()
    {
        if (m_Prefab != null)
        {
            print(m_Time);
            if (m_Time <= 0)
            {
                Instantiate(m_Prefab,new Vector3(transform.position.x,transform.position.y),Quaternion.identity);
                m_Time = m_PeriodicTiming;
            }
            if (m_Time > 0)
            {
                m_Time -= Time.deltaTime;
            }
        }
    }
}

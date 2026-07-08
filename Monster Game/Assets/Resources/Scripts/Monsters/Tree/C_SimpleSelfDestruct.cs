using UnityEngine;

public class C_SimpleSelfDestruct : MonoBehaviour
{
    [SerializeField] private float m_DeathTime;
    public float DeathTime { get { return m_DeathTime; } set { m_DeathTime = value; } }
    void Start()
    {
        Destroy(gameObject, m_DeathTime);
    }
}

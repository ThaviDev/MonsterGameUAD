using UnityEngine;

public class C_SimpleSelfDestruct : MonoBehaviour
{
    [SerializeField] private float m_DeathTime;
    void Start()
    {
        Destroy(gameObject, m_DeathTime);
    }
}

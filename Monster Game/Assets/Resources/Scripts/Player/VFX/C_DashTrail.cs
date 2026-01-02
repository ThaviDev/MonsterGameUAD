using UnityEngine;

public class C_DashTrail : MonoBehaviour
{
    public bool m_startTrail;
    [SerializeField] private ParticleSystem m_ParticleSystem;
    private void Start()
    {
        m_ParticleSystem.Clear();
    }
    void Update()
    {
        if (m_startTrail)
        {
            m_ParticleSystem.Play();
            m_ParticleSystem.Clear();
            m_startTrail = false;
        }
    }
}

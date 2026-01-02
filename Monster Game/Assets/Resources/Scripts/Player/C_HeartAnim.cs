using UnityEngine;

public class C_HeartAnim : MonoBehaviour
{
    [SerializeField] C_PlayerStats m_HeartRate;
    private float m_BPM;
    private Animator m_Animator;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_BPM = m_HeartRate.GetBPM;
        m_Animator.speed = m_BPM/60f;
    }
}

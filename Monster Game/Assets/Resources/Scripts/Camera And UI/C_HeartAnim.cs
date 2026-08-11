using UnityEngine;

public class C_HeartAnim : MonoBehaviour
{
    [SerializeField] C_PlayerStats m_HeartRate;
    private float m_BPM;
    private Animator m_Animator;
    private AudioSource m_AudioSource;
    private void Awake()
    {
        m_HeartRate = FindAnyObjectByType<C_PlayerStats>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_HeartRate == null) return;
        m_BPM = m_HeartRate.GetBPM;
        m_Animator.speed = m_BPM/60f;
        m_AudioSource.pitch = m_BPM/85f;
    }
    public void TriggerBPMSound()
    {
        m_AudioSource.Play();
    }
}

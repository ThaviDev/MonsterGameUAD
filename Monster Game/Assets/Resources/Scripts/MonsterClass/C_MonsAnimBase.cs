using UnityEngine;

public class C_MonsAnimBase : MonoBehaviour
{
    [SerializeField] protected Animator m_Anim;
    [SerializeField] protected SpriteRenderer m_SprRend;
    protected bool m_IsIdle;
    protected bool m_IsSpawning;
    public bool IsSpawning { get { return m_IsSpawning; } }
    protected bool m_IsDespawning;
    public bool IsDespawning { get { return m_IsDespawning; } }
    //[SerializeField] public Color[] m_AnimColors = new Color[6];

    protected virtual void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_SprRend = GetComponent<SpriteRenderer>();
    }
    public virtual void AnimSpawn()
    {
        if (m_Anim == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }
        m_Anim.SetBool("IsSpawned", true);
        m_IsSpawning = true;
    }
    public virtual void TriggerAnimSpawned()
    {
        m_IsSpawning = false;
    }
    public virtual void AnimDespawn()
    {
        if (m_Anim == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }
        m_Anim.SetBool("IsSpawned", false);
        m_IsDespawning = true;
    }
    public virtual void TriggerAnimDespawned()
    {
        m_IsDespawning = false;
    }
    public virtual void AnimChase()
    {
        if (m_Anim == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }
        m_Anim.SetBool("IsAggressive", true);
    }
    public virtual void AnimStealth()
    {
        if (m_Anim == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }
        m_Anim.SetBool("IsAggressive", false);
    }
}

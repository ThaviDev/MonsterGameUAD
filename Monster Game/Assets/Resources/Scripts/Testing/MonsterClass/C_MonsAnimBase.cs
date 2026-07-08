using UnityEngine;

public class C_MonsAnimBase : MonoBehaviour
{
    [SerializeField] protected Animator m_Anim;
    [SerializeField] protected SpriteRenderer m_SprRend;
    protected bool m_IsIdle;
    protected bool m_IsSpawning;
    protected bool m_IsDespawning;
    [SerializeField] public Color[] m_AnimColors = new Color[6];

    protected virtual void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_SprRend = GetComponent<SpriteRenderer>();
    }
    public virtual void AnimSpawn()
    {
        m_Anim.SetBool("IsSpawned", true);
    }
    public virtual void AnimDespawn()
    {
        m_Anim.SetBool("IsSpawned", false);
    }
    public virtual void AnimChase()
    {
        m_Anim.SetBool("IsAggressive", true);
    }
    public virtual void AnimStealth()
    {
        m_Anim.SetBool("IsAggressive", false);
    }
}

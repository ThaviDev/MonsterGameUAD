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
        m_SprRend.color = m_AnimColors[0];
    }
    public virtual void AnimDespawn()
    {
        print ("Despawn Color");
        m_SprRend.color = m_AnimColors[1];
    }
    public virtual void AnimIdle()
    {
        m_SprRend.color = m_AnimColors[2];
    }
    public virtual void AnimChase()
    {
        m_SprRend.color = m_AnimColors[3];
    }
    public virtual void AnimStealth()
    {
        m_SprRend.color = m_AnimColors[4];

    }
    public virtual void AnimAbility()
    {
        m_SprRend.color = m_AnimColors[5];
    }
}

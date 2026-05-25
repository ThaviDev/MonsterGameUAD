using UnityEngine;

public class C_MonsAnimBase : MonoBehaviour
{
    [SerializeField] protected Animator m_Anim;
    [SerializeField] protected SpriteRenderer m_SprRend;
    protected bool m_IsIdle;
    protected bool m_IsSpawning;
    protected bool m_IsDespawning;
    [SerializeField] public Color[] m_AnimColors = new Color[6];

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_SprRend = GetComponent<SpriteRenderer>();
    }
    public void AnimSpawn()
    {
        m_SprRend.color = m_AnimColors[0];
    }
    public void AnimDespawn()
    {
        print ("Despawn Color");
        m_SprRend.color = m_AnimColors[1];
    }
    public void AnimIdle()
    {
        m_SprRend.color = m_AnimColors[2];
    }
    public void AnimChase()
    {
        m_SprRend.color = m_AnimColors[3];

    }
    public void AnimStealth()
    {
        m_SprRend.color = m_AnimColors[4];

    }
    public void AnimAbility()
    {
        m_SprRend.color = m_AnimColors[5];
    }
}

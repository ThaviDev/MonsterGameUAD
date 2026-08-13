using Spine.Unity;
using Spine;
using System.Collections;
using UnityEngine;

public class C_SpineColorChange : MonoBehaviour
{
    private SkeletonAnimation m_SkeletonAnimation;
    private Renderer m_Renderer;
    [Header("Damage")]
    [SerializeField] private Color m_ColorDamage = Color.red;
    [SerializeField] private float m_TimeDamage = 0.1f;
    [Header("Panic")]
    [SerializeField] private Color m_ColorPanic1 = Color.blue;
    [SerializeField] private Color m_ColorPanic2 = Color.cyan;
    [SerializeField] private float m_TimePanic = 0.1f;
    private bool m_IsPanic = false;
    private Color m_ColorOG;
    private void Awake()
    {
        if (m_SkeletonAnimation == null)
            m_SkeletonAnimation = GetComponent<SkeletonAnimation>();
        if (m_Renderer == null)
            m_Renderer = GetComponent<MeshRenderer>();

        m_ColorOG = GetSkeletonColor();
    }
    void Start()
    {
        m_ColorOG = m_Renderer.material.color;
        SetSkeletonColor(m_ColorOG);
    }
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnHit();
        }
        */
    }
    public void OnPanic()
    {
        if (m_SkeletonAnimation == null) return;
        StopAllCoroutines();
        m_IsPanic = true;
        StartCoroutine(PanicCycle());
    }
    IEnumerator PanicCycle()
    {
        while (m_IsPanic)
        {
            SetSkeletonColor(m_ColorPanic1);
            yield return new WaitForSeconds(m_TimePanic);
            SetSkeletonColor(m_ColorOG);
            yield return new WaitForSeconds(m_TimePanic);
            SetSkeletonColor(m_ColorPanic2);
            yield return new WaitForSeconds(m_TimePanic);
            SetSkeletonColor(m_ColorOG);
            yield return new WaitForSeconds(m_TimePanic);
        }
        SetSkeletonColor(m_ColorOG);
    }
    public void StopPanic()
    {
        if (m_SkeletonAnimation == null) return;
        m_IsPanic = false;
        StopAllCoroutines();
        SetSkeletonColor(m_ColorOG);
    }
    public void OnHit()
    {
        if (m_SkeletonAnimation == null) return;
        StopAllCoroutines();
        StartCoroutine(DoHit());
    }
    IEnumerator DoHit()
    {
        //m_Renderer.material.color = m_ColorRed;
        SetSkeletonColor(m_ColorDamage);
        yield return new WaitForSeconds(m_TimeDamage);
        SetSkeletonColor(m_ColorOG);
        //m_Renderer.material.color = m_ColorOG;
    }
    private void SetSkeletonColor(Color color)
    {
        if (m_SkeletonAnimation == null) return;
        m_SkeletonAnimation.Skeleton.SetColor(color);
    }
    private Color GetSkeletonColor()
    {
        return new Color(
            m_SkeletonAnimation.Skeleton.GetColor().r,
            m_SkeletonAnimation.Skeleton.GetColor().g,
            m_SkeletonAnimation.Skeleton.GetColor().b,
            m_SkeletonAnimation.Skeleton.GetColor().a
        );
    }
}

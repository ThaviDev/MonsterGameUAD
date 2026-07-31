using Spine.Unity;
using Spine;
using System.Collections;
using UnityEngine;

public class C_SpineColorChange : MonoBehaviour
{
    private SkeletonAnimation m_SkeletonAnimation;
    private Renderer m_Renderer;
    [SerializeField] private Color m_ColorRed = Color.red;
    [SerializeField] private float m_Time = 0.1f;
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
    }
    void OnHit()
    {
        StopAllCoroutines();
        StartCoroutine(DoHit());
    }
    IEnumerator DoHit()
    {
        //m_Renderer.material.color = m_ColorRed;
        SetSkeletonColor(m_ColorRed);
        yield return new WaitForSeconds(m_Time);
        SetSkeletonColor(m_ColorOG);
        //m_Renderer.material.color = m_ColorOG;
    }
    private void SetSkeletonColor(Color color)
    {
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnHit();
        }
    }
}

using UnityEngine;
using Spine.Unity;
using Spine;
using System.Runtime.CompilerServices;

public class C_PlayerSpineMotor : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_SkeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    public string m_boneAimName;
    public Camera m_cam;

    // Un "Material Property Block" cambia propiedades de los materiales (como el color o la luz)
    private MaterialPropertyBlock m_propertyBlock;
    [SerializeField] private Renderer m_renderer;

    Bone m_boneAim;
    void Start()
    {
        m_boneAim = m_SkeletonAnimation.Skeleton.FindBone(m_boneAimName);
        //Debug.Log("Player Spine Motor Start");
        m_SkeletonAnimation.AnimationState.SetAnimation(0, "frente", true);

        m_renderer = GetComponent<Renderer>();
        m_propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        UpdateCursorLocation();
        // TESTING
        if (PlayerInputs.Instance.UseItemBool)
        {
            Front_Walk();
        }
        if (PlayerInputs.Instance.DashBool)
        {
            Idle();
            ResetColor();
        }
    }

    public void SetHitColor(Color color)
    {
        m_renderer.GetPropertyBlock(m_propertyBlock);
        m_propertyBlock.SetColor("_Color", color);
        m_renderer.SetPropertyBlock(m_propertyBlock);
    }
    public void ResetColor()
    {
        m_renderer.SetPropertyBlock(null);
    }

    private void UpdateCursorLocation()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var skeletonSpacePoint = m_SkeletonAnimation.transform.InverseTransformPoint(cursorPos);

        skeletonSpacePoint.x *= m_SkeletonAnimation.Skeleton.ScaleX;
        skeletonSpacePoint.y *= m_SkeletonAnimation.Skeleton.ScaleY;
        m_boneAim.SetLocalPosition(skeletonSpacePoint);
        //transform.position = new Vector3(cursorPos.x, cursorPos.y, 0);
    }
    private void Front_Walk()
    {
        m_SkeletonAnimation.AnimationState.AddAnimation(0, "Side_walk", true, 0);
    }
    private void Idle()
    {
        m_SkeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0);
    }
}

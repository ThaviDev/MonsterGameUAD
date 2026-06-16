using UnityEngine;
using Spine.Unity;
using Spine;

public class C_PlayerSpineMotor : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_SkeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    public string m_boneAimName;
    public Camera m_cam;

    Bone m_boneAim;
    void Start()
    {
        m_boneAim = m_SkeletonAnimation.Skeleton.FindBone(m_boneAimName);
        Debug.Log("Player Spine Motor Start");
        m_SkeletonAnimation.AnimationState.SetAnimation(0, "frente", true);
    }

    void Update()
    {
        UpdateCursorLocation();
        if (PlayerInputs.Instance.UseItemBool)
        {
            Front_Walk();
        }
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
        m_SkeletonAnimation.AnimationState.AddAnimation(0, "caminar", true, 0);
    }
}

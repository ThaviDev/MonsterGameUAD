using UnityEngine;
using Spine;
using Spine.Unity;

public class C_BoneFollower : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    //public string boneName = "tu-hueso";
    //private Bone targetBone;

    [SpineBone(dataField: "skeletonAnimation")]
    public string m_boneName;

    Bone m_bone;

    void Start()
    {
        if (skeletonAnimation == null)
            skeletonAnimation = GetComponent<SkeletonAnimation>();

        if (skeletonAnimation == null)
        {
            Debug.LogError("C_BoneFollower: SkeletonAnimation component not found.");
            return;
        }

        // Buscar el hueso por su nombre
        m_bone = skeletonAnimation.Skeleton.FindBone(m_boneName);
        if (m_bone == null)
            Debug.LogError($"Hueso '{m_boneName}' no encontrado.");
    }

    void Update()
    {
        if (m_bone == null) return;

        Debug.Log($"Bone Position: {m_bone.GetWorldPosition(skeletonAnimation.transform)}, Bone Rotation: {m_bone.GetQuaternion()}");

        Vector3 bonePosition = m_bone.GetWorldPosition(skeletonAnimation.transform);
        transform.position = bonePosition;

        transform.rotation = m_bone.GetQuaternion();
    }
}

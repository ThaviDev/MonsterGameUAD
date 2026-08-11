using UnityEngine;
using Spine.Unity;
using Spine;
using System.Runtime.CompilerServices;

public class C_PlayerSpineMotor : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_SkeletonAnimation;
    [SerializeField] private Rigidbody2D m_Rb;

    [SpineBone(dataField: "skeletonAnimation")]
    public string m_boneAimName;
    //public Camera m_cam;

    // Un "Material Property Block" cambia propiedades de los materiales (como el color o la luz)
    private MaterialPropertyBlock m_propertyBlock;
    [SerializeField] private Renderer m_renderer;

    private bool m_MoveCheck = false;

    Bone m_boneAim;
    void Start()
    {
        m_boneAim = m_SkeletonAnimation.Skeleton.FindBone(m_boneAimName);

        //Debug.Log("Player Spine Motor Start");
        m_SkeletonAnimation.AnimationState.SetAnimation(0, "frente", true);

        m_renderer = GetComponent<Renderer>();
        m_propertyBlock = new MaterialPropertyBlock();
        Idle(0);
    }

    void Update()
    {
        UpdateCursorLocation();
        UpdateWalk();
        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            Front_Walk(0);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Idle(0);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Front_Walk(1);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Idle(1);
        }*/

    }

    private void UpdateWalk()
    {
        // Esta funcion es inapropiada, llegar a cambiar en un mejor sistema despues
        if (m_Rb == null)
        {
            Debug.LogWarning("Rigidbody2D no esta asignado para spine, imposible saber velocidad del jugador");
            return;
        }
        var velocity = m_Rb.linearVelocity;
        float speed = velocity.magnitude;
        bool isMoving = speed > 0.1f;
        //Debug.Log("Speed: " + speed + ", IsMoving: " + isMoving);
        if (isMoving != m_MoveCheck)
        {
            //Debug.Log("Movement state changed. IsMoving: " + isMoving);
            if (isMoving)
            {
                Front_Walk(0);
            }
            else
            {
                Idle(0);
            }
            m_MoveCheck = isMoving;
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
    private void Front_Walk(int i)
    {
        m_SkeletonAnimation.AnimationState.SetAnimation(i, "Side_walk", true);
    }
    private void Idle(int i)
    {
        m_SkeletonAnimation.AnimationState.SetAnimation(i, "idle", true);
    }
}

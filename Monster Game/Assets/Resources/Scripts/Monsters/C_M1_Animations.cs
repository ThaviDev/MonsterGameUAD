using UnityEngine;

public class C_M1_Animations : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    private bool m_StartGrabAnimation;
    private bool m_GrabPlayer;
    private bool m_HasPlayer;
    //private bool m_ReleasePlayer;

    public bool StartGrabAnimation { set { m_StartGrabAnimation = value; } }
    public bool GrabPlayer { get { return m_GrabPlayer; } set { m_GrabPlayer = value; } }
    public bool HasPlayer { set { m_HasPlayer = value; } }
    //public bool ReleasePlayer { set { m_ReleasePlayer = value; } }
    void Start()
    {

    }
    void Update()
    {
        if (m_HasPlayer)
        {
            m_Animator.SetBool("HasPlayer", true);
        }
        else
        {
            m_Animator.SetBool("HasPlayer", false);
        }

        if (m_StartGrabAnimation)
        {
            m_Animator.SetBool("Grab", true);
        }
        else
        {
            m_Animator.SetBool("Grab", false);
        }
        if (GrabPlayer)
        {
            m_StartGrabAnimation = false;
        }
    }
    // Esta funcion es accedida por un Animation Event
    public void GrabTrigger()
    {
        m_GrabPlayer = true;
        //m_StartGrabAnimation = false;
    }
}

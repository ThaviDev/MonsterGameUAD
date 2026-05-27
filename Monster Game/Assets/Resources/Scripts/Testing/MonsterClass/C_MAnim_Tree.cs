using UnityEngine;

public class C_MAnim_Tree : C_MonsAnimBase
{
    private C_Monst_Tree m_TreeMotor;
    //private bool m_StartGrabAnim;
    private bool m_GrabPlayer;
    private bool m_isPlayerGrabed; // Funcion de Motor para
    //private bool m_ReleasePlayer;
    //private bool m_StartSlamAnim;
    private bool m_Slam;
    //private bool m_StartChargeAnim;
    private bool m_Charge;

    public bool GrabPlayer { get { return m_GrabPlayer; } set { m_GrabPlayer = value; } }
    public bool IsPlayerGrabed { set { m_isPlayerGrabed = value; } }
    public bool Slam { get { return m_Slam; } set { m_Slam = value; } }
    public bool Charge { get { return m_Charge; } set { m_Charge = value; } }
    protected override void Awake()
    {
        base.Awake();
        m_TreeMotor = GetComponentInParent<C_Monst_Tree>();
    }
    private void Update()
    {
        if (!m_isPlayerGrabed)
        {
            AnimationRelease();
        }
    }
    public override void AnimSpawn()
    {
        base.AnimSpawn();
    }
    public override void AnimIdle()
    {
        base.AnimIdle();
    }
    public void AnimationGrab()
    {
        m_Anim.SetBool("Grab", true);
    }
    public void TriggerGrabFunction()
    {
        m_GrabPlayer = true;
    }
    private void AnimationRelease()
    {
        m_Anim.SetBool("Grab", false);
    }
    public void AnimationSlam()
    {
        m_Anim.SetBool("Slam", true);
    }
    public void TriggerSlamFunction()
    {
        m_Slam = true;
    }
    public void AnimationCharge()
    {
        m_Anim.SetBool("Charge", true);
    }
    public void TriggerChargeFunction()
    {
        m_Charge = true;
    }
}

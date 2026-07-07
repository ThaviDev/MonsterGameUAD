using UnityEngine;

public class C_MAnim_Tree : C_MonsAnimBase
{
    private C_Monst_Tree m_TreeMotor;
    //private bool m_StartGrabAnim;
    private bool m_GrabPlayer;
    private bool m_isPlayerGrabed;
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
        if (m_isPlayerGrabed)
        {
            m_Anim.SetBool("HasPlayer", true);
        } else
        {
            m_Anim.SetBool("HasPlayer", false);
        }
    }
    public override void AnimSpawn()
    {
        base.AnimSpawn();
    }
    public void AnimScream()
    {
        m_Anim.SetBool("IsScreaming", true);
    }
    public void AnimNotScreaming()
    {
        m_Anim.SetBool("IsScreaming", false);
    }
    public void AnimGrab()
    {
        m_Anim.SetBool("Grabing", true);
    }
    public void AnimGrabRelease()
    {
        m_Anim.SetBool("Grabing", false);
        m_GrabPlayer = false;
    }
    public void AnimSlam()
    {
        m_Anim.SetBool("Slamimg", true);
    }
    public void AnimCharge()
    {
        m_Anim.SetBool("Charging", true);
    }
    public void TriggerChargeFunction()
    {
        m_Charge = true;
    }
}

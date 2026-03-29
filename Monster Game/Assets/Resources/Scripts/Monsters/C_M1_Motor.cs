using UnityEngine;

public class C_M1_Motor : C_Monster
{
    [Header("Tree Settings")]
    [SerializeField] C_M1_Animations m_AnimMan;
    [SerializeField] GameObject m_PlayerGrabbed;
    [SerializeField] bool m_DidIGrabPlayer;
    // Expose layer mask so it's editable in Inspector; fallback to "Player" in Start.
    [SerializeField] LayerMask m_PlayerLayerMask;

    [SerializeField] public Vector2 m_PlayerGrabbedPosition;

    [Header("Grab Ability Settings")]
    [SerializeField] Vector2 m_ColOffset = new Vector2(-1, 0);
    [SerializeField] Vector2 m_ColSize = new Vector2(2, 2);
    protected override void Start()
    {
        base.Start();
        // El codigo de aqui aplicara sin acceder a nada de otro monstruo
        if (m_PlayerLayerMask == 0)
        {
            m_PlayerLayerMask = LayerMask.GetMask("Player");
        }
    }
    protected override void Update()
    {
        base.Update();
        // El codigo de aqui aplicara sin acceder a nada de otro monstruo
        // Animation system drives the grab attempt via GrabPlayer flag.
        if (m_AnimMan != null && m_AnimMan.GrabPlayer)
        {
            TryGrab();
            m_AnimMan.GrabPlayer = false;
        }
        // Liberacion de agarre por accion del jugador (mash out)
        if (m_PlayerAction1)
        {
            ReleaseGrab();
            m_PlayerAction1 = false;
        }
    }

    // Try to find a player inside the configured box and capture the first hit.
    void TryGrab()
    {
        ReleaseGrab(); // reset previous state

        Vector2 center = (Vector2)transform.position + m_ColOffset;
        Collider2D hit = Physics2D.OverlapBox(center, m_ColSize, 0f, m_PlayerLayerMask);
        if (hit != null)
        {
            print("Grabbed player: " + hit.gameObject.name);
            if (m_AnimMan != null)
                m_AnimMan.HasPlayer = true;
            m_PlayerGrabbed = hit.gameObject;
            m_PlayerGrabbed.GetComponent<C_PlayerMotor>()?.GetGrabbed(this,m_PlayerGrabbedPosition, true);
            //m_JugadorCapturado.GetComponent<C_PlayerMotor>()?.ChangeState(new CapturedState(m_JugadorCapturado.GetComponent<C_PlayerMotor>()));
            //m_JugadorCapturado.transform.position = transform.position; // snap to hugger position; in a real scenario you'd want to lerp this or use a joint
            m_DidIGrabPlayer = true;
        }
        else
        {
            if (m_AnimMan != null)
                m_AnimMan.HasPlayer = false;
        }
    }

    public void ReleaseGrab()
    {
        m_PlayerGrabbed = null;
        m_DidIGrabPlayer = false;
        if (m_AnimMan != null)
            m_AnimMan.HasPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check layer against configured mask and only trigger a new grab if not already holding someone.
        if (((1 << other.gameObject.layer) & (int)m_PlayerLayerMask) != 0 && !m_DidIGrabPlayer)
        {
            if (m_AnimMan != null)
                m_AnimMan.StartGrabAnimation = true;
        }
    }
}

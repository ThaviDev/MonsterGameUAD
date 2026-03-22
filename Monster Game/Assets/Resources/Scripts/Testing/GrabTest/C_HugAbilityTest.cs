using UnityEngine;

public class C_HugAbilityTest : MonoBehaviour
{
    [SerializeField] C_HugAnimationManTest m_AnimMan;
    [SerializeField] Vector2 m_ColOffset = new Vector2(-1, 0);
    [SerializeField] Vector2 m_ColSize = new Vector2(2, 2);

    [SerializeField] GameObject m_JugadorCapturado;
    [SerializeField] bool m_AtrapeJugador;

    // Expose layer mask so it's editable in Inspector; fallback to "Player" in Start.
    [SerializeField] LayerMask m_PlayerLayerMask;

    [SerializeField] Vector2 m_PlayerGrabbedPosition;
    [SerializeField] public Vector2 PlayerGrabbedPosition { get { return m_PlayerGrabbedPosition; } }

    void Start()
    {
        if (m_PlayerLayerMask == 0)
            m_PlayerLayerMask = LayerMask.GetMask("Player");
    }

    void Update()
    {
        // Animation system drives the grab attempt via GrabPlayer flag.
        if (m_AnimMan != null && m_AnimMan.GrabPlayer)
        {
            TryGrab();
            m_AnimMan.GrabPlayer = false;
        }

        // debug / manual release
        if (Input.GetKeyDown(KeyCode.H))
        {
            Release();
            if (m_AnimMan != null)
                m_AnimMan.HasPlayer = false;
        }
    }

    // Try to find a player inside the configured box and capture the first hit.
    void TryGrab()
    {
        Release(); // reset previous state

        Vector2 center = (Vector2)transform.position + m_ColOffset;
        Collider2D hit = Physics2D.OverlapBox(center, m_ColSize, 0f, m_PlayerLayerMask);
        if (hit != null)
        {
            m_JugadorCapturado = hit.gameObject;
            m_JugadorCapturado.GetComponent<C_PlayerMotor>()?.GetGrabbed(this, true);
            //m_JugadorCapturado.GetComponent<C_PlayerMotor>()?.ChangeState(new CapturedState(m_JugadorCapturado.GetComponent<C_PlayerMotor>()));
            //m_JugadorCapturado.transform.position = transform.position; // snap to hugger position; in a real scenario you'd want to lerp this or use a joint
            m_AtrapeJugador = true;
            if (m_AnimMan != null)
                m_AnimMan.HasPlayer = true;
        }
        else
        {
            if (m_AnimMan != null)
                m_AnimMan.HasPlayer = false;
        }
    }

    public void Release()
    {
        m_JugadorCapturado = null;
        m_AtrapeJugador = false;
        if (m_AnimMan != null)
            m_AnimMan.HasPlayer = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + m_ColOffset;
        Gizmos.DrawWireCube(center, m_ColSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check layer against configured mask and only trigger a new grab if not already holding someone.
        if (((1 << other.gameObject.layer) & (int)m_PlayerLayerMask) != 0 && !m_AtrapeJugador)
        {
            if (m_AnimMan != null)
                m_AnimMan.StartGrabAnimation = true;
        }
    }
}

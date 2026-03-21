using UnityEngine;

public class C_HugAbilityTest : MonoBehaviour
{
    [SerializeField] C_HugAnimationManTest m_AnimMan;
    Collider2D m_ColCercaniaJugador;

    [SerializeField] Vector2 m_ColOffset = new Vector2(-1,0);
    [SerializeField] Vector2 m_ColSize = new Vector2(2,2);

    [SerializeField] GameObject m_JugadorCapturado;
    [SerializeField] bool m_DetecteJugador;
    [SerializeField] bool m_AtrapeJugador;
    int m_playerLayerMask;

    void Start()
    {
        m_playerLayerMask = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (m_DetecteJugador && !m_AtrapeJugador)
        {
            m_AnimMan.StartGrabAnimation = true;
            m_DetecteJugador = false;
        }
        if (m_AnimMan.GrabPlayer)
        {
            Grabbing();
            m_AnimMan.GrabPlayer = false;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            m_AnimMan.HasPlayer = false;
        }
    }

    void Grabbing()
    {
        // Para testing
        m_AtrapeJugador = false;
        m_JugadorCapturado = null;

        Vector2 center = (Vector2)transform.position + m_ColOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, m_ColSize, 0, m_playerLayerMask);

        foreach (Collider2D hit in hits) {
            C_PlayerMotor pMotor = hit.GetComponent<C_PlayerMotor>();

            if (pMotor != null) {
                m_JugadorCapturado = hit.gameObject;
                m_AtrapeJugador = true;
                m_AnimMan.HasPlayer = true;
                break;
            }
        }
        if (!m_AtrapeJugador)
        {
            m_AnimMan.HasPlayer = false;
        }
    }

    void Release()
    {
        m_JugadorCapturado = null;
        m_AtrapeJugador = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + m_ColOffset;
        Gizmos.DrawWireCube(center, m_ColSize);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Esta madre si es sacada de la IA de google no tengo ni idea que es "1 <<"
        if (((1 << other.gameObject.layer) & m_playerLayerMask) != 0)
        {
            m_DetecteJugador = true;
        }
    }
}

using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public static Action<Collider2D> OnPyrHit;
    public static Action OnPanic;
    public static Action OnRelax;
    public static Action OnPyrDeath;

    // Temporal por clase de VFX
    public GameObject m_vfx_MonsterScream;

    // Temporal referencia de PlayerStats para obtener BMP
    // Luego hay que cambiar esa clase a un SCOB
    public C_PlayerStats m_playerStats;

    // Temporal FeedbackQueue del sprite del jugador
    public SpriteRenderer m_sprite;

    [SerializeField] private bool m_IsInPanic;

    [SerializeField] private bool m_IsInvincible;

    Collider2D _yCol;
    void Start()
    {
        
    }

    void Update()
    {
        if (m_playerStats.GetBPM >= m_playerStats.GetPanicThreshold && !m_IsInPanic)
        {
            m_IsInPanic = true;
            OnPanic?.Invoke();
            // Temporal Feedback Queue
            m_sprite.color = Color.blue;
        }
        if (m_playerStats.GetBPM <= m_playerStats.GetRelaxThreshold && m_IsInPanic)
        {
            m_IsInPanic = false;
            OnRelax?.Invoke();
            // Temporal Feedback Queue
            m_sprite.color = Color.white;
        }
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPyrHit?.Invoke();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.layer == 6) // Monster Layer
        {
            OnPyrHit?.Invoke(otherCol);
            if (m_playerStats.GetBPM >= 150)
            {
                OnPyrDeath?.Invoke();
            }
            if (m_IsInPanic)
            {
                OnPyrDeath?.Invoke();
            }

            // Temporal para clase de VFX
            Instantiate(m_vfx_MonsterScream, new Vector3(otherCol.transform.position.x, otherCol.transform.position.y + 1.6f), Quaternion.identity);
        }
    }
}

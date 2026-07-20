using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_stamina = 100f;
    [SerializeField] private float m_staminaRegenRate = 5f;
    [SerializeField] private float m_staminaConsumptionRate = 10f;
    [SerializeField] private float m_maxStamina = 100f;

    [Header("Stats")]
    [SerializeField] private float m_health = 100f;
    [SerializeField] private float m_maxHealth = 100f;

    public float Health
    {
        get { return m_health; }
        set { m_health = Mathf.Clamp(value, 0, m_maxHealth); }
    }

    public float Stamina
    {
        get { return m_stamina; }
        set { m_stamina = Mathf.Clamp(value, 0, m_maxStamina); }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

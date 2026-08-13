using UnityEngine;

public class C_Hazzard : MonoBehaviour
{
    [SerializeField] private Collider2D m_Collider;
    [SerializeField] private float m_Damage = 10f;
    [SerializeField] private float m_Fear = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out C_PlayerMotor pyrMotor))
        {
            return;
        }
        C_PlayerMotor.OnPyrHit?.Invoke(m_Collider, m_Damage, m_Fear,0.01f,0.01f);
    }
}

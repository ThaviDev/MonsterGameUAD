using UnityEngine;
using Spine.Unity;

public class C_FlipSpineCharacter : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_SkeletonAnimation;
    [SerializeField] private Rigidbody2D m_Rb;
    [SerializeField] private bool m_startLeft = true;

    // Guardamos la última dirección para no estar volteando cada frame innecesariamente
    private float m_currentScaleX;

    void Start()
    {
        // Inicializamos la escala según la dirección inicial
        m_currentScaleX = m_startLeft ? -1f : 1f;
        m_SkeletonAnimation.Skeleton.ScaleX = m_currentScaleX;
    }

    void Update()
    {
        // Usamos linearVelocity como hacías antes
        float velocityX = m_Rb.linearVelocity.x;

        // Umbral pequeño para evitar micro-volteos cuando está quieto
        if (velocityX > 0.1f)
        {
            // Mirando a la derecha
            m_currentScaleX = 1f;
        }
        else if (velocityX < -0.1f)
        {
            // Mirando a la izquierda
            m_currentScaleX = -1f;
        }
        else
        {
            // (Opcional) Si quieres que se quede mirando hacia la última dirección cuando está idle, no hagas nada aquí.
            // Si prefieres que siempre mire hacia el mouse cuando está quieto, borra este 'else'.
            return;
        }

        // Aplicamos el volteo SOLO si ha cambiado
        if (m_SkeletonAnimation.Skeleton.ScaleX != m_currentScaleX)
        {
            m_SkeletonAnimation.Skeleton.ScaleX = m_currentScaleX;
        }
    }
}
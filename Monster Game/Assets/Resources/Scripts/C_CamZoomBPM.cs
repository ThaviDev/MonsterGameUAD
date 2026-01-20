using UnityEngine;
public class C_CamZoomBPM : MonoBehaviour
{
    [SerializeField] Camera m_Cam;
    [SerializeField] C_PlayerStats m_Heart;
    [SerializeField] float m_SmoothSpeed;

    private float m_TargetSize;
    private float m_CurrentSize;
    private float m_velocity; // Variable interna para el SmoothDamp
    private void Start()
    {
        if (m_Cam != null)
        {
            m_CurrentSize = m_Cam.orthographicSize;
        }
    }
    void Update()
    {
        if (m_Heart != null)
        {
            //m_Cam.orthographicSize = 5f - (m_Heart.GetBPM - 80f) * 0.025f;
            m_TargetSize = 5f - (m_Heart.GetBPM - 80f) * 0.025f;

            // SmoothDamp modifica 'm_velocity' internamente
            // Para el próximo frame, 'm_velocity' ya tiene el valor actualizado
            m_CurrentSize = Mathf.SmoothDamp(m_CurrentSize, m_TargetSize, ref m_velocity, m_SmoothSpeed);

            // Aplicar nuevo tamanio
            m_Cam.orthographicSize = m_CurrentSize;
        }
    }
}

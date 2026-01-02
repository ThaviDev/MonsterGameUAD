using UnityEngine;

public class C_CamZoomBPM : MonoBehaviour
{
    [SerializeField] Camera m_Cam;
    [SerializeField] C_PlayerStats m_Heart;
    void Update()
    {
        if (m_Heart != null)
        m_Cam.orthographicSize = 5f - (m_Heart.GetBPM - 80f) * 0.025f;
    }
}

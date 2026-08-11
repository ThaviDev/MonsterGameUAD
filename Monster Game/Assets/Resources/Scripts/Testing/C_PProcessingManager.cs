using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class C_PProcessingManager : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float m_ppIntensity = 0;
    public float PostProcessingIntensity { set { m_ppIntensity = value; } }

    [SerializeField] private Volume m_myVolume;
    [SerializeField] private ColorAdjustments m_colorAdjustments;
    [SerializeField] private Bloom m_Bloom;

    [Header("Valores")]
    [SerializeField] private float m_ContrastIntensity = 35f;
    [SerializeField] private float m_SatReductionIntensity = 100f;
    [SerializeField] private float m_SaturationDefault = 50f;
    [SerializeField] private float m_BloomIntensity = 2f;

    [Header("Curva de saturación (potencia)")]
    [SerializeField] private float m_SaturationPower = 0.3f; // <1 = inicio rápido

    [Header("Referencia Jugador BPM")]
    [SerializeField] private bool m_UseBPM = false;
    [SerializeField] private C_PlayerStats m_PStats;

    [Header("Referencia Distancia de Monstruo")]
    [SerializeField] private bool m_UseDistance = false;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float distance;
    private void Start()
    {
        m_myVolume = GetComponent<Volume>();
        if (m_myVolume != null)
        {
            m_myVolume.profile.TryGet<ColorAdjustments>(out m_colorAdjustments);
            m_myVolume.profile.TryGet<Bloom>(out m_Bloom);
        }
        if (m_UseBPM && m_PStats == null)
        {
            m_PStats = FindAnyObjectByType<C_PlayerStats>();
        }
    }

    private void Update()
    {
        if (m_colorAdjustments != null)
        {
            m_colorAdjustments.contrast.value = m_ppIntensity * m_ContrastIntensity;

            // Aplicamos la transformación no lineal
            float t = Mathf.SmoothStep(0, 1, m_ppIntensity);
            m_colorAdjustments.saturation.value = (-t * (m_SatReductionIntensity + m_SaturationDefault)) + m_SaturationDefault;
        }

        if (m_Bloom != null)
        {
            m_Bloom.intensity.value = m_ppIntensity * m_BloomIntensity;
        }

        if (m_UseBPM && m_PStats != null)
        {
            float bpm = m_PStats.GetBPM; // Suponiendo que GetBPMIntensity() devuelve un valor entre 0 y 1
            float bpm_max = m_PStats.GetPanicThreshold;
            float bpm_min = m_PStats.GetBPM_Minimum;
            float bpmIntensity = Mathf.Clamp01((bpm - bpm_min) / (bpm_max - bpm_min));
            m_ppIntensity = bpmIntensity;
            //Debug.Log("BPM Intensity: " + bpmIntensity + "Current BPM: " + m_PStats.GetBPM);
            return;
        }
        if (m_UseDistance && pointA != null && pointB != null)
        {
            distance = Vector2.Distance(pointA.position, pointB.position);
            if (distance >= 10)
            {
                m_ppIntensity = 0;
            }
            else
            {
                m_ppIntensity = 1 - (distance / 10f);
            }
            return;
        }
    }
}
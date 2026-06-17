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

    private void Start()
    {
        m_myVolume = GetComponent<Volume>();
        if (m_myVolume != null)
        {
            m_myVolume.profile.TryGet<ColorAdjustments>(out m_colorAdjustments);
            m_myVolume.profile.TryGet<Bloom>(out m_Bloom);
        }
    }

    private void Update()
    {
        if (m_colorAdjustments != null)
        {
            m_colorAdjustments.contrast.value = m_ppIntensity * m_ContrastIntensity;

            // Aplicamos la transformación no lineal
            float t = Mathf.Pow(m_ppIntensity, m_SaturationPower);
            m_colorAdjustments.saturation.value = (-t * (m_SatReductionIntensity + m_SaturationDefault)) + m_SaturationDefault;
        }

        if (m_Bloom != null)
        {
            m_Bloom.intensity.value = m_ppIntensity * m_BloomIntensity;
        }
    }
}
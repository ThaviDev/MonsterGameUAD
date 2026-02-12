using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VolFx;

public class C_PProcessingManager : MonoBehaviour
{
    // Serializado para probar el efecto
    [Range(0,1)]
    [SerializeField] float m_ppIntensity = 0;
    public float SET_PostProcessingIntensity { set { m_ppIntensity = value; } }
    [SerializeField] private Volume m_myVolume;
    [SerializeField] private VhsFx m_VhsEffect;
    [SerializeField] private ColorAdjustments m_colorAdjustments;
    [SerializeField] private Bloom m_Bloom;
    private void Start()
    {
        m_myVolume = GetComponent<Volume>();
        if (m_myVolume != null)
        {
            if (m_myVolume.profile.TryGet<ColorAdjustments>(out m_colorAdjustments))
            {
            }
            if (m_myVolume.profile.TryGet<Bloom>(out m_Bloom))
            {
            }
        }
    }
    private void Update()
    {
        if (m_colorAdjustments != null)
        {
            m_colorAdjustments.contrast.value = m_ppIntensity * 35;
            m_colorAdjustments.saturation.value = (-m_ppIntensity * 150) + 50;
        }
        if (m_Bloom != null)
        {
            m_Bloom.intensity.value = m_ppIntensity * 2;
        }
    }
}

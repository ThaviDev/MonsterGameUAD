using UnityEngine;
using UnityEngine.UI;

public class C_HeartSlider : MonoBehaviour
{
    [SerializeField] C_HeartRate m_HeartRate;
    private float m_DamageTaken;
    private float m_StaminaUsed;
    private float m_Fear;

    private float m_StaminaSliderValue;
    private float m_FearSliderValue;

    [SerializeField] Slider s_HealthSliderLeft;
    [SerializeField] Slider s_HealthSliderRight;
    [SerializeField] Slider s_StaminaSliderLeft;
    [SerializeField] Slider s_StaminaSliderRight;
    [SerializeField] Slider s_FearSliderLeft;
    [SerializeField] Slider s_FearSliderRight;


    void Start()
    {
        
    }

    void Update()
    {
        m_DamageTaken = m_HeartRate.GetDamageTaken;
        m_StaminaUsed = m_HeartRate.GetStaminaUsed;
        m_Fear = m_HeartRate.GetFear;
        m_StaminaSliderValue = m_DamageTaken + m_StaminaUsed;
        m_FearSliderValue = m_DamageTaken + m_StaminaUsed + m_Fear;

        s_HealthSliderLeft.value = m_DamageTaken;
        s_HealthSliderRight.value = m_DamageTaken;
        s_StaminaSliderLeft.value = m_StaminaSliderValue;
        s_StaminaSliderRight.value = m_StaminaSliderValue;
        s_FearSliderLeft.value = m_FearSliderValue;
        s_FearSliderRight.value = m_FearSliderValue;
    }
}

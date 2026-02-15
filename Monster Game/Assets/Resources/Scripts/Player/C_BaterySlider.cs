using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class C_BaterySlider : MonoBehaviour
{
    [SerializeField] private C_FlashLightMotor m_FLight;
    [SerializeField] private Slider m_TipSlider;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TMP_Text m_TextPercentage;

    void Update()
    {
        m_Slider.value = m_FLight.BateryPercentage;
        m_TipSlider.value = m_FLight.BateryPercentage;
        m_TextPercentage.text = Mathf.RoundToInt(m_FLight.BateryPercentage * 100) + "%";
    }
}

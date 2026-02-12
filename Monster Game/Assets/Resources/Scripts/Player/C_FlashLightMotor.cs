using UnityEngine;
using UnityEngine.Rendering.Universal;

public class C_FlashLightMotor : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    private float m_BateryPercentage;
    public float BateryPercentage { get { return m_BateryPercentage; } set { m_BateryPercentage = value; } }
    private Light2D m_FLight;
    [Header("Valores")]
    [SerializeField] private float m_LightIntensity = 2f;
    [SerializeField] private float m_MinimumLight = 0.5f;
    [SerializeField] private float m_LightRange = 5f;
    [SerializeField] private float m_MinimumRange = 3f;
    void Start()
    {
        m_FLight = GetComponent<Light2D>();
    }

    void Update()
    {
        if (m_BateryPercentage != 0)
        {
            m_FLight.intensity = (m_BateryPercentage * (m_LightIntensity - m_MinimumLight)) + m_MinimumLight;
            m_FLight.pointLightInnerRadius = (m_BateryPercentage * m_LightRange) + m_MinimumRange;
            m_FLight.pointLightOuterRadius = (m_BateryPercentage * m_LightRange) + m_MinimumRange + 0.5f;
        } else
        {
            m_FLight.intensity = 0f;
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class C_FlashLightMotor : MonoBehaviour
{
    [SerializeField]
    //[Range(0,1)]
    private float m_BateryPercentage;
    //private FloatSCOB scob_bateryPercentage;
    public float BateryPercentage { get { return m_BateryPercentage; } set { m_BateryPercentage = value; } }
    [SerializeField] private float m_BateryUsageReduction;
    private bool m_IsTurnedOn;
    public bool IsFlashLightOn { get { return m_IsTurnedOn; } set { m_IsTurnedOn = value; } }
    private Light2D m_FLight;
    [Header("Valores")]
    [SerializeField] private float m_LightIntensity = 2f;
    [SerializeField] private float m_MinimumLight = 0.5f;
    [SerializeField] private float m_LightRange = 5f;
    [SerializeField] private float m_MinimumRange = 3f;
    void Start()
    {
        m_FLight = GetComponent<Light2D>();
        m_BateryPercentage = 1f;
    }

    void Update()
    {
        if (m_BateryPercentage > 1)
        {
            m_BateryPercentage = 1;
        } 
        if (m_BateryPercentage < 0)
        {
            m_BateryPercentage = 0;
        }
        bool TurnFlashLight = PlayerInputs.Instance.FlashLightBool;
        if (TurnFlashLight)
        {
            if (m_IsTurnedOn)
            {
                m_IsTurnedOn = false;
            } else
            {
                m_IsTurnedOn = true;
            }
        }
        if (m_BateryPercentage != 0)
        {
            if (m_IsTurnedOn)
            {
                // Reducir porcentaje de bateria
                m_BateryPercentage -= (Time.deltaTime / 1000) * m_BateryUsageReduction;
                // Prender Luz (Intensidad y Rango)
                m_FLight.intensity = (m_BateryPercentage * (m_LightIntensity - m_MinimumLight)) + m_MinimumLight;
                m_FLight.pointLightInnerRadius = (m_BateryPercentage * m_LightRange) + m_MinimumRange;
                m_FLight.pointLightOuterRadius = (m_BateryPercentage * m_LightRange) + m_MinimumRange + 0.5f;
            }
            else
            {
                m_FLight.intensity = 0f;
            }
        }
        else
        {
            if (m_IsTurnedOn)
            {
                m_IsTurnedOn = false;
            }
            m_FLight.intensity = 0f;
        }
    }
}

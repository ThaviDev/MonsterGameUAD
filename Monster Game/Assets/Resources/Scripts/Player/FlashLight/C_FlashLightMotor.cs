using UnityEngine;
using UnityEngine.Rendering.Universal;

public class C_FlashLightMotor : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    private float m_BateryPercentage;
    //private FloatSCOB scob_bateryPercentage;
    public float BateryPercentage { get { return m_BateryPercentage; } set { m_BateryPercentage = value; } }
    [SerializeField] private float m_BateryUsageReduction;
    private bool m_IsTurnedOn;
    public bool IsFlashLightOn { get { return m_IsTurnedOn; } set { m_IsTurnedOn = value; } }
    private Light2D m_FLight;
    [Header("Intensidad de luz")]
    [SerializeField] private float m_LightIntensity = 2f;
    [SerializeField] private float m_MaximumLight = 2f;
    [SerializeField] private float m_MinimumLight = 0.5f;

    [Header("Rango de Longitud de luz")]
    [SerializeField] private float m_LightRange = 5f;
    public float LightRange { get { return m_LightRange; } }
    [SerializeField] private float m_MaximumRange = 5f;
    [SerializeField] private float m_MinimumRange = 3f;
    [SerializeField] private float m_OuterRangeExcess = 0.5f;

    [Header("Angulo de luz")]
    [SerializeField] private float m_LightAngle = 70;
    public float LightAngle {  get { return m_LightAngle; } }
    [SerializeField] private float m_MaximumAngle = 70;
    [SerializeField] private float m_MinimumAngle = 10;
    [SerializeField] private float m_OuterAngleExcess = 70;
    void Start()
    {
        m_FLight = GetComponent<Light2D>();
        m_BateryPercentage = 1f;
    }

    void Update()
    {
        if (m_BateryPercentage > 1)
            m_BateryPercentage = 1;
        if (m_BateryPercentage < 0)
            m_BateryPercentage = 0;

        if (m_BateryPercentage == 0)
        {
            m_FLight.intensity = 0f;
            m_IsTurnedOn = false;
        }

        bool TurnFlashLight = PlayerInputs.Instance.FlashLightBool;
        if (TurnFlashLight)
        {
            m_IsTurnedOn = !m_IsTurnedOn;
            /*
            if (m_IsTurnedOn)
            {
                m_IsTurnedOn = false;
            } else
            {
                m_IsTurnedOn = true;
            }
            */
        }
        if (m_IsTurnedOn)
        {
            ReduceBateryOnUse();
            SetLightValues();
        }
        else
        {
            m_FLight.intensity = 0f;
        }
    }

    private void ReduceBateryOnUse()
    {
        // Reducir porcentaje de bateria
        m_BateryPercentage -= (Time.deltaTime / 1000) * m_BateryUsageReduction;
    }

    private void SetLightValues()
    {
        m_LightIntensity = (m_BateryPercentage * (m_MaximumLight - m_MinimumLight)) + m_MinimumLight;
        m_LightRange = (m_BateryPercentage * m_MaximumRange) + m_MinimumRange;
        m_LightAngle = (m_BateryPercentage * m_MaximumAngle) + m_MinimumAngle;

        // Prender Luz (Intensidad y Rango)

        m_FLight.intensity = m_LightIntensity;
        m_FLight.pointLightInnerRadius = m_LightRange;
        m_FLight.pointLightOuterRadius = m_LightRange + m_OuterRangeExcess;
        m_FLight.pointLightInnerAngle = m_LightAngle;
        m_FLight.pointLightOuterAngle = m_LightAngle + m_OuterAngleExcess;
    }

    public void AddBatteryPercent(float rechargeAmount)
    {
        m_BateryPercentage += rechargeAmount;
        if (m_BateryPercentage > 1)
            m_BateryPercentage = 1;
    }
}

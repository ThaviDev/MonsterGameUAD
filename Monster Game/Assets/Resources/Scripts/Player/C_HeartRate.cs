using UnityEngine;

public class C_HeartRate : MonoBehaviour
{
    private float m_DamageTaken;
    private float m_DamageRegen = 2f;
    private float m_DamageRegenGoal;

    private float m_StaminaUsed;
    private float m_LastStaminaUsed;
    private float m_StaminaRegen = 2f;

    private float m_Fear;
    private float m_FearRegen = 1f;

    private float m_MaxStat = 120; // Maximum Amount of value of Fear, Damage, Stamina

    private float m_BPM;
    private float m_BPM_DefaultValue = 80;
    private float m_BPM_Minimum = 80;
    private float m_BPM_Maximum = 200;
    private float m_BPM_PanicThreshold = 180;
    /* Umbral el cual detona el estado de pánico al llegar o superar este valor*/
    private float m_BPM_RelaxThreshold = 160;
    /* Umbral minimo de BPM que tiene que pasar el BPM para salir del estado de pánico*/

    private float m_StabilizeStaminaTime_DefaultValue = 1.5f;
    private float m_StabilizeStaminaTime = 0;
    /* Tiempo que tiene que pasar para que la estmina se regenere al no utilizarla */

    public float GetBPM { get { return m_BPM; } }
    public float GetDamageTaken { get { return m_DamageTaken; } }
    public float GetStaminaUsed { get { return m_StaminaUsed; } }
    public float GetFear { get { return m_Fear; } }

    void Start()
    {

    }

    void Update()
    {
        // Los beats por minuto dependen de 3 estadisticas y su minimo
        m_BPM = m_DamageTaken + m_StaminaUsed + m_Fear + m_BPM_Minimum;

        Fear();
        Stamina();
        DamageTaken();

        if (Input.GetKeyDown(KeyCode.V))
        {
            m_StaminaUsed += 20;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            m_Fear += 20;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            m_DamageTaken += 33;
            m_DamageRegenGoal += 33;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            m_DamageRegenGoal -= 20;
        }

        print("Stamina: " + m_StaminaUsed + " DamageTaken: " + m_DamageTaken + " Fear: " + m_Fear);
        print("Stabilize Stamina: " + m_StabilizeStaminaTime);

        // 
    }

    void Fear()
    {
        if (m_Fear > 0)
        {
            m_Fear -= m_FearRegen * Time.deltaTime;
        }
        if (m_Fear < 0)
        {
            m_Fear = 0;
        }
    }
    void Stamina()
    {
        // Regeneracion de estamina
        if (m_StaminaUsed > 0 && m_StabilizeStaminaTime == 0)
        {
            m_StaminaUsed -= m_StaminaRegen * Time.deltaTime;
        }
        if (m_StaminaUsed < 0)
        {
            m_StaminaUsed = 0;
        }

        // Esperar a estabilizacion de estamina en segundos
        if (m_StabilizeStaminaTime > 0)
        {
            m_StabilizeStaminaTime -= Time.deltaTime;
        }
        if (m_StabilizeStaminaTime < 0)
        {
            m_StabilizeStaminaTime = 0;
        }

        // Si el valor de estamina AUMENTO desestabiliza la estamina
        if (m_StaminaUsed > m_LastStaminaUsed)
        {
            m_StabilizeStaminaTime = m_StabilizeStaminaTime_DefaultValue;
            m_LastStaminaUsed = m_StaminaUsed;
            print("Mi valor cambio");
        }
        else
        {
            // Manten actualizado el valor
            m_LastStaminaUsed = m_StaminaUsed;
            print("actualizo valor");
        }
    }
    void DamageTaken()
    {
        if (m_DamageTaken > m_DamageRegenGoal)
        {
            m_DamageTaken -= m_DamageRegen * Time.deltaTime;
        }
        if (m_DamageRegenGoal < 0)
        {
            m_DamageRegenGoal = 0;
        }
    }
}

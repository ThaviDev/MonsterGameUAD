using UnityEngine;

public class C_PlayerStats : MonoBehaviour
{
    [SerializeField] C_PlayerMovement m_PyrMoveScript;

    [SerializeField] float m_DamageTaken;
    private float m_DamageRegen = 2f;
    private float m_DamageRegenGoal;

    [SerializeField] float m_Fear;
    private float m_FearRegen = 1f;
    private float m_FearWhenHitDefault = 10f;

    [SerializeField] float m_Stamina;
    private float m_LastStamina;
    private float m_StaminaRegen = 2f;
    private float m_CurStaminaMoveUse;

    private float m_MaxStat = 120; // Maximum Amount of value of Fear, Damage and Stamina. Min is 0

    [SerializeField] float m_BPM;
    private float m_BPM_DefaultValue = 80;
    private float m_BPM_Minimum = 80;
    private float m_BPM_Maximum = 200;
    private float m_BPM_PanicThreshold = 180;
    /* Umbral el cual detona el estado de pánico al llegar o superar este valor*/
    private float m_BPM_RelaxThreshold = 160;
    /* Umbral minimo de BPM que tiene que pasar el BPM para salir del estado de pánico*/

    /* Tiempo que tiene que pasar para que la estmina se regenere al no utilizarla */
    [SerializeField] float m_StabilizeStaminaTimeDefaultV = 1.5f;
    private float m_StabilizeStaminaTime = 0;
    /* Tiempo donde el jugador no puede recibir danio despues de recibir danio */
    [SerializeField] float m_InvincibleTimeDefaultV = 1f;
    float m_InvincibleTime;

    //[SerializeField] float m_BateryPercent;

    /* Determina el estado de velocidad del jugador
     * 0 = Idle
     * 1 = Caminata Normal
     * 2 = Trotar
     * 3 = Correr
     */
    [SerializeField] float[] m_SpeedLevels;
    [SerializeField] float[] m_AccelLevels; // Acceleration Levels
    [SerializeField] float[] m_DecelLevels; // Deceleration Levels
    [SerializeField] float[] m_StaminaUseLevels; // Uso de estamina en niveles

    private float m_CurSpeedGoal;
    private float m_CurAcceleration;
    private float m_CurDeceleration;

    private float m_LastSpeedGoal;
    private float m_CurSpeed;
    private float m_SpeedReduction;



    /* ---- Comunicar informacion de variables a otros codigos ---- */
    public float GetBPM { get { return m_BPM; } }
    public float GetBPM_Maximum { get { return m_BPM_Maximum; } }
    public float GetBPM_Minimum { get { return m_BPM_Minimum; } }
    public float GetDamageTaken { get { return m_DamageTaken; } }
    public float GetStaminaUsed { get { return m_Stamina; } }
    public float GetFear { get { return m_Fear; } }
    public float GetCurrentSpeedGoal { get { return m_CurSpeedGoal; } }
    public float GetCurrentAcceleration { get { return m_CurAcceleration; } }
    public float GetCurrentDeceleration { get { return m_CurDeceleration; } }

    public float GetPanicThreshold { get { return m_BPM_PanicThreshold; } }
    public float GetRelaxThreshold { get { return m_BPM_RelaxThreshold; } }

    void Start()
    {
        //m_BateryPercent = 1; // 1 = 100%
        m_InvincibleTime = 0;
        C_PlayerMotor.OnPyrHit += PyrHitEvent;
        C_PlayerMotor.OnScreamedAt += RecieveFearFromPlayerGotScreamedAt;
    }

    void PyrHitEvent(Collider2D collider, float damage, float fear, float stunDuration, float knockbackForce)
    {
        RecieveDamage(collider, damage,fear,stunDuration);
    }

    void Update()
    {
        // Establecer los niveles de velocidad dependiendo de Player Move
        m_CurSpeedGoal = m_SpeedLevels[m_PyrMoveScript.GetMovementStatus];
        m_CurAcceleration = m_AccelLevels[m_PyrMoveScript.GetMovementStatus];
        m_CurDeceleration = m_DecelLevels[m_PyrMoveScript.GetMovementStatus];
        m_CurStaminaMoveUse = m_StaminaUseLevels[m_PyrMoveScript.GetMovementStatus];

        // Los beats por minuto dependen de 3 estadisticas y su minimo
        m_BPM = m_DamageTaken + m_Stamina + m_Fear + m_BPM_Minimum;

        //print("Velocidad: " + m_CurSpeedGoal + " Uso de estamina: " + m_CurStaminaMoveUse);

        Fear();
        Stamina();
        DamageTaken();
        LimitMaxBPM();

        InvincibilityFrames();

        // TESTING
        if (Input.GetKeyDown(KeyCode.V))
        {
            m_Stamina += 20;
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

        //print("Stamina: " + m_StaminaUsed + " DamageTaken: " + m_DamageTaken + " Fear: " + m_Fear);
        //print("Stabilize Stamina: " + m_StabilizeStaminaTime);

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
        if (m_Fear >= m_MaxStat)
        {
            m_Fear = m_MaxStat;
        }
    }
    void Stamina()
    {
        // Gasto por movimiento
        if (m_CurStaminaMoveUse > 0 && m_Stamina < m_MaxStat)
        {
            // Gasto estamina
            m_Stamina += Time.deltaTime * m_CurStaminaMoveUse;
        }

        // Regeneracion de estamina
        if (m_Stamina > 0 && m_StabilizeStaminaTime == 0 && m_CurStaminaMoveUse <= 0)
        {
            m_Stamina -= m_StaminaRegen * Time.deltaTime;
        }
        if (m_Stamina < 0)
        {
            m_Stamina = 0;
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

        // Si el valor de estamina AUMENTO, desestabiliza la estamina
        if (m_Stamina > m_LastStamina)
        {
            m_StabilizeStaminaTime = m_StabilizeStaminaTimeDefaultV;
            m_LastStamina = m_Stamina;
        }
        else
        {
            // Manten actualizado el valor
            m_LastStamina = m_Stamina;
        }
        if (m_Stamina == m_MaxStat)
        {
            m_Stamina = m_MaxStat;
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
        if (m_DamageTaken >= m_MaxStat)
        {
            m_DamageTaken = m_MaxStat;
        }
    }

    void InvincibilityFrames()
    {
        if (m_InvincibleTime > 0)
        {
            m_InvincibleTime -= Time.deltaTime;
        }
    }

    private void LimitMaxBPM()
    {
        if (m_BPM > m_BPM_Maximum)
        {
            float exceso = m_BPM - m_BPM_Maximum;

            if (m_Fear > 0f)
            {
                float reduccionMiedo = Mathf.Min(m_Fear, exceso);
                m_Fear -= reduccionMiedo;
                exceso -= reduccionMiedo;
            }

            if (exceso > 0f && m_Stamina > 0f)
            {
                float reduccionEstamina = Mathf.Min(m_Stamina, exceso);
                m_Stamina -= reduccionEstamina;
                exceso -= reduccionEstamina;
            }

            if (exceso > 0f && m_DamageTaken > 0f)
            {
                float reduccionDanio = Mathf.Min(m_DamageTaken, exceso);
                m_DamageTaken -= reduccionDanio;
                exceso -= reduccionDanio;
            }
        }
    }
    public void UseStamina(float staminaAmount)
    {
        m_Stamina += staminaAmount;
        if (m_Stamina == m_MaxStat)
        {
            m_Stamina = m_MaxStat;
        }
        m_StabilizeStaminaTime = m_StabilizeStaminaTimeDefaultV;
        m_LastStamina = m_Stamina;
    }
    public void RecieveDamage(Collider2D otherCol, float damageAmount)
    {
        if (m_InvincibleTime <= 0)
        {
            m_DamageTaken += damageAmount;
            m_DamageRegenGoal += damageAmount;
            m_Fear += m_FearWhenHitDefault;
            m_InvincibleTime = m_InvincibleTimeDefaultV;
        }
    }
    public void RecieveDamage(Collider2D otherCol, float damageAmount, float fearHitAmount)
    {
        if (m_InvincibleTime <= 0)
        {
            m_DamageTaken += damageAmount;
            m_DamageRegenGoal += damageAmount;
            m_Fear += fearHitAmount;
            m_InvincibleTime = m_InvincibleTimeDefaultV;
        }
    }
    public void RecieveDamage(Collider2D otherCol, float damageAmount, float fearHitAmount, float invincibleTime)
    {
        if (m_InvincibleTime <= 0)
        {
            m_DamageTaken += damageAmount;
            m_DamageRegenGoal += damageAmount;
            m_Fear += fearHitAmount;
            m_InvincibleTime = invincibleTime;
        }
    }
    private void RecieveFearFromPlayerGotScreamedAt(C_MonsterMotor otherMonster, float fearAmount)
    {
        RecieveFear(fearAmount);
    }
    public void RecieveFear(float fearAmount)
    {
        m_Fear += fearAmount;
    }

    private void OnDestroy()
    {
        C_PlayerMotor.OnPyrHit -= PyrHitEvent;
        C_PlayerMotor.OnScreamedAt -= RecieveFearFromPlayerGotScreamedAt;
    }
}

using UnityEngine;
using UnityEngine.Rendering;

/*
 la rotacion del enemigo es con un peso?
donde estan los steering behaviours que va usar el enemigo?
donde esta el prefab del player y el enemigo?
"El monstruo está siendo iluminado por el jugador", eso no ya esta implementado?
te parece si todo hereda de la clase Entity?
Estas usando el new input system?

Que tanta libertad tengo de ir cambiando los scripts, puedo arregalar unas cosas que facilitan el codigo tanto para design como programacion,
como por ejemplo hay scrpts cortos que cabnen en un solo script (como flip sprite movement, player animation)(el palyer interaction , que mejor sea un solo script que manage los colliders)



Crear la clase Item 
*/


public class Q_Enemy : Q_Entity
{
    #region STATS
    private Vector3 direction = Vector3.zero;
    public Vector3 m_direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private Vector3 force = Vector3.zero;
    public Vector3 m_force
    {
        get { return force; }
        set { force = value; }
    }

    private float speed = 10.0f;
    public float m_speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private uint healthPoints = 1;
    public uint m_healthPoints
    {
        get { return healthPoints; }
        set { healthPoints = value; }
    }

    private bool isOnLight = false;
    public bool m_isOnLight
    {
        get { return isOnLight; }
        set { isOnLight = value; }
    }

    #endregion

    #region ENERGY_SYSTEM
    private float maxEnergy = 100;
    public float m_maxEnergy
    {
        get { return m_maxEnergy; }
        set { m_maxEnergy = value; }
    }

    private float currentEnergy = 0;
    public float m_currentEnergy
    {
        get { return m_currentEnergy; }
        set { m_currentEnergy = value; }
    }

    private float energyRegenSpeed = 1.2f;
    public float m_energyRegenSpeed
    {
        get { return m_energyRegenSpeed; }
        set { m_energyRegenSpeed = value; }
    }

    private float energyWasteSpeed = 1;
    public float m_energyWasteSpeed
    {
        get { return m_energyWasteSpeed; }
        set { m_energyWasteSpeed = value; }
    }

    private bool energyIsRegen = false;

    #endregion

    protected virtual void Start()
    {
        base.Start();

        currentEnergy = maxEnergy;
    }

    protected virtual void Update()
    {
        base.Update();

        if (true == energyIsRegen)
        {
            RegenEnergy();
        }
        else
        {
            WasteEnergy();
        }


    }

    protected virtual void Attack()
    {

    }

    protected void WasteEnergy()
    {
        currentEnergy -= energyWasteSpeed * Time.deltaTime;
        if (currentEnergy <= 0)
        {
            energyIsRegen = true;
        }
    }
    protected void RegenEnergy()
    {
        currentEnergy += energyRegenSpeed * Time.deltaTime;
        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
            energyIsRegen = false;
        }
    }

    protected float getPlayerDistance()
    {
        return 0.0f;
    }
}

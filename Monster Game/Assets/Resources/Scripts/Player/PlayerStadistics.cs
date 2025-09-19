using System;
using UnityEngine;

public class PlayerStadistics : MonoBehaviour
{
    public static Action OnPyrDeath;

    [SerializeField] PlayerMovement _pyrMoveScript;
    [SerializeField] float _staminaUseMult;
    [SerializeField] float _staminaRestMult;

    [SerializeField] float _maxStamina;
    [SerializeField] float _maxHealth;

    [SerializeField] float[] _speedLevels;
    [SerializeField] float[] _accelLevels; // Acceleration Levels
    [SerializeField] float[] _decelLevels; // Deceleration Levels
    [SerializeField] float[] _staminaUseLevels; // Uso de estamina en niveles
    //[SerializeField] float _normalMaxSpeed = 5f;
    //public float GetNormalMaxSpeed { get { return _normalMaxSpeed; } }
    //[SerializeField] float _runMaxSpeed = 10f;
    //public float GetRunningMaxSpeed { get { return _runMaxSpeed; } }
    //[SerializeField] float _normalAcceleration = 3f;
    //public float GetNormalAcceleration { get { return _normalAcceleration; } }
    //[SerializeField] float _runAcceleration = 5f;
    //public float GetRunningAcceleration { get { return _runAcceleration; } }
    float _curDeceleration;
    public float GetCurrentDeceleration { get { return _curDeceleration; } }

    private float _curMaxSpeed;
    public float GetCurrentMaxSpeed
    {
        get { return _curMaxSpeed; }
    }
    private float _curAcceleration;
    public float GetCurrentAcceleration
    {
        get { return _curAcceleration; }
    }
    private float _curStaminaUse;

    bool _isUsingStamina;
    bool _canGetHit;
    [SerializeField] float _hitDamage;
    float _stamRegenCooldownT;
    float _stamRegenRate;

    [SerializeField] PlayerMotor _pm;
    [SerializeField] FloatSCOB _pyrStamina;
    [SerializeField] FloatSCOB _pyrHealth;
    [SerializeField] IntSCOB _pyrScrapAmount;
    void Start()
    {
        _pyrScrapAmount.SCOB_Value = 0;
        _pyrStamina.SCOB_Value = _maxStamina;
        _pyrHealth.SCOB_Value = _maxHealth;
        _curMaxSpeed = _speedLevels[0];

        _canGetHit = true;
        PlayerMotor.OnPyrHit += PlayerWasHit;
        OnPyrDeath += PlayerDied;
    }
    void Update()
    {
        _maxStamina = _pyrHealth.SCOB_Value;
        _curMaxSpeed = _speedLevels[_pyrMoveScript.GetMovementStatus];
        _curAcceleration = _accelLevels[_pyrMoveScript.GetMovementStatus];
        _curDeceleration = _decelLevels[_pyrMoveScript.GetMovementStatus];
        _curStaminaUse = _staminaUseLevels[_pyrMoveScript.GetMovementStatus];

        if (_pyrStamina.SCOB_Value > 0)
        {
            _pyrStamina.SCOB_Value += Time.deltaTime * _curStaminaUse;
        } 
        else
        {
            if (_curStaminaUse > 0) // Es positivo
            {
                _pyrStamina.SCOB_Value += Time.deltaTime * _curStaminaUse;
            } else
            {
                _pyrHealth.SCOB_Value += Time.deltaTime * _curStaminaUse;
            }
        }

        if (_pyrStamina.SCOB_Value >= _maxStamina)
        {
            _pyrStamina.SCOB_Value = _maxStamina;
        }

        /*
        switch (_pyrMoveScript.GetMovementStatus)
        {
            default:
                print("No hay un estado claro de velocidad");
                break;
            case 0: // Puntillas / Quieto
                if (_pyrStamina.SCOB_Value < _maxStamina)
                {
                    _pyrStamina.SCOB_Value += Time.deltaTime * _staminaRestMult;
                }
                else
                {
                    _pyrStamina.SCOB_Value = _maxStamina;
                }
                _curMaxSpeed = _normalMaxSpeed;
                break;
            case 1: // Caminata
                _curMaxSpeed = _runMaxSpeed;
                _pyrStamina.SCOB_Value -= Time.deltaTime * _staminaUseMult;
                break;
            case 2: // Trotar
                break;
            case 3: // Correr
                break;
            case 4:  // Sprintear
                break;

        }
        */
    }
    public void IncreasePlayerSpeed()
    {
        
    }

    public void ResetPlayerSpeed()
    {

    }

    void PlayerDied()
    {
        _canGetHit = false;
    }
    private void PlayerWasHit(Collider2D otherCol)
    {
        if (!_canGetHit) {
            return;
        }
        _pyrHealth.SCOB_Value -= _hitDamage;
        if (_pyrStamina.SCOB_Value > _pyrHealth.SCOB_Value)
        {
            _pyrStamina.SCOB_Value = _pyrHealth.SCOB_Value;
        }

        if (_pyrHealth.SCOB_Value < 0)
        {
            _pyrHealth.SCOB_Value = 0;
        }

        if (_pyrHealth.SCOB_Value == 0)
        {
            OnPyrDeath?.Invoke();
        }
    }
    private void OnDestroy()
    {
        PlayerMotor.OnPyrHit -= PlayerWasHit;
        OnPyrDeath -= PlayerDied;
    }
}

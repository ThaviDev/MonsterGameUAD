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

    [SerializeField] float _normalMaxSpeed = 5f;
    public float GetNormalMaxSpeed { get { return _normalMaxSpeed; } }
    [SerializeField] float _runMaxSpeed = 10f;
    public float GetRunningMaxSpeed { get { return _runMaxSpeed; } }
    [SerializeField] float _normalAcceleration = 3f;
    public float GetNormalAcceleration { get { return _normalAcceleration; } }
    [SerializeField] float _runAcceleration = 5f;
    public float GetRunningAcceleration { get { return _runAcceleration; } }
    [SerializeField] float _deceleration = 3f;
    public float GetDeceleration { get { return _deceleration; } }

    private float _curMaxSpeed;
    public float GetCurrentMaxSpeed
    {
        get { return _curMaxSpeed; }
    }

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
        _curMaxSpeed = _normalMaxSpeed;

        _canGetHit = true;
        PlayerMotor.OnPyrHit += PlayerWasHit;
        OnPyrDeath += PlayerDied;
    }
    void Update()
    {
        _maxStamina = _pyrHealth.SCOB_Value;
        switch (_pyrMoveScript.GetMovementStatus)
        {
            case 2:
                _curMaxSpeed = _runMaxSpeed;
                _pyrHealth.SCOB_Value -= Time.deltaTime * _staminaUseMult;
                break;
            case 1: // Run
                _curMaxSpeed = _runMaxSpeed;
                _pyrStamina.SCOB_Value -= Time.deltaTime * _staminaUseMult;
                break;
            case 0: // Walk
                if (_pyrStamina.SCOB_Value < _maxStamina)
                {
                    _pyrStamina.SCOB_Value += Time.deltaTime * _staminaRestMult;
                } else
                {
                    _pyrStamina.SCOB_Value = _maxStamina;
                }
                _curMaxSpeed = _normalMaxSpeed;
                break;
            default:
                break;
        }
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

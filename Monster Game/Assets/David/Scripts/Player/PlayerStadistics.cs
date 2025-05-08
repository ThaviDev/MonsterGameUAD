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

    [SerializeField] float _normalSpeed = 5f;
    [SerializeField] float _runSpeed = 10f;
    private float _curSpeed;
    public float GetSpeed
    {
        get { return _curSpeed; }
    }

    bool _isUsingStamina;
    bool _canGetHit;
    [SerializeField] float _hitDamage;
    float _stamRegenCooldownT;
    float _stamRegenRate;

    [SerializeField] PlayerMotor _pm;
    [SerializeField] FloatSCOB _pyrStamina;
    [SerializeField] FloatSCOB _pyrHealth;
    void Start()
    {
        _pyrStamina.SCOB_Value = _maxStamina;
        _pyrHealth.SCOB_Value = _maxHealth;
        _curSpeed = _normalSpeed;

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
                _curSpeed = _runSpeed;
                _pyrHealth.SCOB_Value -= Time.deltaTime * _staminaUseMult;
                break;
            case 1: // Run
                _curSpeed = _runSpeed;
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
                _curSpeed = _normalSpeed;
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

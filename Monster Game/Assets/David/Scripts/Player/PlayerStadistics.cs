using System;
using UnityEngine;

public class PlayerStadistics : MonoBehaviour
{
    public static Action OnPyrDeath;

    float _maxStamina;
    float _maxHealth;

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
        _pyrHealth.SCOB_Value = 100;
        _canGetHit = true;
        PlayerMotor.OnPyrHit += PlayerWasHit;
        OnPyrDeath += PlayerDied;
    }
    void Update()
    {
    }

    void PlayerDied()
    {
        _canGetHit = false;
    }
    private void PlayerWasHit(Collider2D otherCol)
    {
        print("Si me pegan cierto?");
        if (!_canGetHit) {
            return;
        }
        _pyrHealth.SCOB_Value -= _hitDamage;

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

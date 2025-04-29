using UnityEngine;

public class PlayerStadistics : MonoBehaviour
{
    float _maxStamina;
    float _maxHealth;

    bool _isUsingStamina;
    float _stamRegenCooldownT;
    float _stamRegenRate;

    FloatSCOB _pyrStamina;
    FloatSCOB _pyrHealth;
    void Start()
    {
        PlayerMotor.OnPyrHit += PlayerWasHit;
    }
    void Update()
    {
        
    }
    private void PlayerWasHit()
    {
        print("Bajar Vida");
    }
}

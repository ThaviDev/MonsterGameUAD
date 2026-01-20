using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator _animator;
    int _movementStatus;
    [SerializeField] PlayerMovement _Pmovement;

    void Start()
    {
        
    }
    void Update()
    {
        _movementStatus = _Pmovement.GetMovementStatus;
        switch (_movementStatus)
        {
            case 0:
                _animator.SetBool("IsIdle",true);
                break;
            case 1:
                _animator.SetBool("IsIdle",false);
                break;
            default:
                print("Not valid status for anim");
                break;
        }
    }
}

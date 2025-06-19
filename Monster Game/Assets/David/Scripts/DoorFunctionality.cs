using UnityEngine;

public class DoorFunctionality : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _closeTime;
    [SerializeField] private bool _isVertical;
    private bool _openDoorCallOnce;
    private bool _closeDoorCallOnce;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        _animator.SetBool("IsVertical", _isVertical);

    }
    void Update()
    {
        if (_closeTime > 0)
        {
            _closeTime -= Time.deltaTime;
        }
        
        if (_closeTime <= 0 && _closeDoorCallOnce)
        {
            CloseDoor();
        }

        if (_openDoorCallOnce)
        {
            OpenDoor();
        }

    }

    void OpenDoor()
    {
        _openDoorCallOnce = false;
        _collider.enabled = false;
        _animator.SetBool("IsOpen",true);
    }

    void CloseDoor()
    {
        _openDoorCallOnce = false;
        _collider.enabled = true;
        _animator.SetBool("IsOpen",false);
    }

    public void OpenDoorCall(float timeAmount)
    {
        _closeTime = timeAmount;
        _openDoorCallOnce = true;
        _closeDoorCallOnce = true;
    }
}

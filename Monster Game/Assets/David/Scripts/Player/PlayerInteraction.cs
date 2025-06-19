using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float _interRadius = 3f;
    [SerializeField] float _OpenTimeDoor = 2f;
    void Start()
    {
        
    }
    void Update()
    {
        var interactBtn = PlayerInputs.OnInteractPressed();
        if (interactBtn == true)
        {
            print("Interactuo");
            Interact();
        }

    }

    void Interact()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _interRadius);
        foreach (Collider2D col in hitColliders)
        {
            if (col.gameObject.GetComponent<DoorFunctionality>())
            {
                var door = col.gameObject.GetComponent<DoorFunctionality>();
                door.OpenDoorCall(_OpenTimeDoor);
            }
            if (col.gameObject.GetComponent<AlarmFunction>())
            {
                var alarm = col.gameObject.GetComponent<AlarmFunction>();
                alarm.ActivateAlarmCall();
            }
        }
    }
}

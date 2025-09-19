using UnityEngine;

public class DoorFinalFunction : MonoBehaviour
{
    [SerializeField] Collider2D _col;
    [SerializeField] Animator _animator;
    public void OpenDoor()
    {
        _col.enabled = false;
        _animator.SetBool("IsOpen", true);
    }
}

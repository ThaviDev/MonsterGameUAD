using UnityEngine;

public class C_DistantRotatingLinesCursor : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform player;
    //[SerializeField] SpriteRenderer cursorSpriteRenderer;

    private Vector3 mousePosition;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        UpdateCursorPosition();
        UpdateCursorRotation();
    }

    void UpdateCursorPosition()
    {
        transform.position = new Vector3(player.position.x,player.position.y + 0.5f);
    }

    void UpdateCursorRotation()
    {
        if (player != null)
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;
            Vector2 direction = (transform.position - cursorPos).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + -90);
        }
    }
}

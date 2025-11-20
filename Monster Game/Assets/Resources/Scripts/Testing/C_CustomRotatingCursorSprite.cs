using UnityEngine;

public class C_CustomRotatingCursorSprite : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer cursorSpriteRenderer;

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
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0;
        transform.position = cursorPos;
    }

    void UpdateCursorRotation()
    {
        if (player != null)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + -90);
        }
    }
}
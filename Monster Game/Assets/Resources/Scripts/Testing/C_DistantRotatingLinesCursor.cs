using UnityEngine;

public class C_DistantRotatingLinesCursor : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform m_PlayerAim;

    void Start()
    {
        //Cursor.visible = false;
    }

    void Update()
    {
        UpdateCursorPosition();
        UpdateCursorRotation();
    }

    void UpdateCursorPosition()
    {
        // Se hace un simple offset vertical para que el cursor este en el centro del personaje en vez de en los pies
        //transform.position = new Vector3(player.position.x,player.position.y + 0.4f);
        transform.position = new Vector3(m_PlayerAim.position.x, m_PlayerAim.position.y);
    }

    void UpdateCursorRotation()
    {
        if (m_PlayerAim != null)
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;
            // Se hace un simple offset vertical para que el cursor este en el centro del personaje en vez de en los pies
            Vector2 direction = (transform.position - new Vector3(cursorPos.x, cursorPos.y)).normalized;
            //Vector2 direction = (transform.position - new Vector3(cursorPos.x, cursorPos.y + 0.4f)).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + -90);
        }
    }
}

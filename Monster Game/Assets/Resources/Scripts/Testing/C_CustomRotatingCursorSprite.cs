using UnityEngine;
using UnityEngine.UI;

public class C_CustomRotatingCursorSprite : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform m_PlayerAim;
    //[SerializeField] SpriteRenderer cursorSpriteRenderer;
    //[SerializeField] Image cursorSpriteRenderer;

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
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //cursorPos.z = 0;
        //transform.position = cursorPos;
        // Se hace un simple offset vertical para que el cursor este en el centro del personaje en vez de en los pies
        //transform.position = new Vector3(cursorPos.x,cursorPos.y + 0.4f,0);
        transform.position = new Vector3(cursorPos.x,cursorPos.y,0);
    }

    void UpdateCursorRotation()
    {
        if (m_PlayerAim != null)
        {
            // Se hace un simple offset vertical para que el cursor este en el centro del personaje en vez de en los pies
            Vector2 direction = (transform.position - new Vector3(m_PlayerAim.position.x, m_PlayerAim.position.y)).normalized;
            //Vector2 direction = (transform.position - new Vector3(player.position.x,player.position.y + 0.4f)).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + -90);
        }
    }
}
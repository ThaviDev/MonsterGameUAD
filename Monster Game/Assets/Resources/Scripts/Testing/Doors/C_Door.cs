using UnityEngine;

public class C_Door : MonoBehaviour
{
    [SerializeField] protected float m_OpenTime;
    protected float m_CurOpenTime;
    [SerializeField] protected Sprite m_OpenDoorSprite;
    [SerializeField] protected Sprite m_ClosedDoorSprite;
    protected SpriteRenderer m_Renderer;
    protected Collider2D m_Col;
    public virtual void Awake()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Col = GetComponent<Collider2D>();
    }
    public virtual void Start()
    {
        CloseDoor();
    }

    public virtual void Update()
    {
        if (m_CurOpenTime > 0)
        {
            m_CurOpenTime -= Time.deltaTime;
            OpenDoor();
        }
        if (m_CurOpenTime <= 0)
        {
            m_CurOpenTime = 0;
            CloseDoor();
        }
    }
    public virtual void OpenDoor()
    {
        m_Renderer.sprite = m_OpenDoorSprite;
        m_Col.enabled = false;
    }
    public virtual void CloseDoor()
    {
        m_Renderer.sprite = m_ClosedDoorSprite;
        m_Col.enabled = true;
    }
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            m_CurOpenTime = m_OpenTime;
        }
    }
}

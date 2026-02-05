using UnityEngine;

public class C_JScare_Test : MonoBehaviour
{
    public GameObject m_JS_Reference;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_JS_Reference.SetActive(true);
        }
    }
}

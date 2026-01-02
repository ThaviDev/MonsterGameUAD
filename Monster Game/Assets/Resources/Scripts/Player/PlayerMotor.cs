using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public static Action<Collider2D> OnPyrHit;
    // Temporal por clase de VFX
    public GameObject m_vfx_MonsterScream;

    [SerializeField] private bool m_IsInvincible;

    Collider2D _yCol;
    void Start()
    {
        
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnPyrHit?.Invoke();
        }*/
    }

    private void OnTriggerEnter2D(Collider2D otherCol)
    {
        if (otherCol.gameObject.layer == 6) // Monster Layer
        {
            OnPyrHit.Invoke(otherCol);
            // Temporal para clase de VFX
            Instantiate(m_vfx_MonsterScream, new Vector3(otherCol.transform.position.x, otherCol.transform.position.y + 1.6f), Quaternion.identity);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class C_DoorLocked : C_Door
{
    //[SerializeField] private Dictionary m_keyValues;
    [SerializeField] private string m_KeyReq;
    [SerializeField] private bool m_IsLocked;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        m_IsLocked = true;
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OpenDoor()
    {
        base.OpenDoor();
    }
    public override void CloseDoor()
    {
        base.CloseDoor();
    }
    public override void OnCollisionEnter2D(Collision2D other)
    {
        //base.OnCollisionEnter2D(other);
        if (m_IsLocked)
        {
            if (other.gameObject.TryGetComponent(out C_InventoryManager inventory))
            {
                Debug.Log("Player Tiene Inventory, vamos a checar si tiene la llave");
                inventory.CheckIfHasKey(m_KeyReq, this);
            } else
            {
                Debug.Log("Player no tiene Inventory");
            }
        } else
        {
            base.OnCollisionEnter2D(other);
        }
    }
    public void UnlockDoor()
    {
        m_IsLocked = false;
    }
}

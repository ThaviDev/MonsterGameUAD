using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventorySlot
{
    public C_ItemSCOB m_CurItem;
}

public class C_InventoryManager : MonoBehaviour
{
    public List<InventorySlot> m_InvSlots;
    public Action OnInventoryChange;
    public void AddItem(C_PickUpItem item)
    {
        InventorySlot slot = new InventorySlot();
        slot.m_CurItem = item.m_ItemData;
        // Temporal
        m_InvSlots.Add(slot);
    }

    public void CheckIfHasKey(string KeyName,C_DoorLocked door)
    {
        foreach (InventorySlot slot in m_InvSlots)
        {
            Debug.Log("Checamos en slot " + slot);
            if (slot.m_CurItem.m_ItemName == KeyName)
            {
                Debug.Log("Si tienes la llave!");
                door.UnlockDoor();
                m_InvSlots.Remove(slot);
                break;
            }
            Debug.Log("mmmm este no es");
        }
    }
}

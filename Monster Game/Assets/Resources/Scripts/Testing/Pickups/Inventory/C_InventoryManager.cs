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
    [SerializeField] private int m_MaxSlots = 4;
    public int MaxSlots { get { return m_MaxSlots; } }
    [SerializeField] private List<InventorySlot> m_InvSlots;
    public List<InventorySlot> InvSlots { get { return m_InvSlots; } }
    public Action OnInventoryChange;
    public bool InventoryHasSpace()
    {
        print(m_InvSlots);
        if (m_InvSlots.Count >= m_MaxSlots)
        {
            return false;
        } else
        {
            return true;
        }
    }
    public void AddItem(C_PickUpItem item)
    {
        InventorySlot slot = new InventorySlot();
        slot.m_CurItem = item.m_ItemData;
        m_InvSlots.Add(slot);
        OnInventoryChange?.Invoke();
    }

    public void CheckIfHasKey(string KeyName,C_DoorLocked door)
    {
        foreach (InventorySlot slot in m_InvSlots)
        {
            Debug.Log("Checamos en slot " + slot);
            if (slot.m_CurItem.m_ItemName == KeyName)
            {
                Debug.Log("Si tienes la llave!");
                door.UsedKeyOnDoor(KeyName);
                m_InvSlots.Remove(slot);
                OnInventoryChange?.Invoke();
                break;
            }
            Debug.Log("mmmm este no es");
        }
    }
}

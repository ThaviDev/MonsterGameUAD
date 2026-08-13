using Spine;
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
    private int m_SelectedSlot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_SelectedSlot = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_SelectedSlot = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_SelectedSlot = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_SelectedSlot = 3;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseItem();
        }
    }
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

    public void UseItem()
    {
        if (m_SelectedSlot < 0 || m_SelectedSlot >= m_InvSlots.Count)
        {
            Debug.LogWarning("Selected slot is out of range.");
            return;
        }
        if (m_InvSlots[m_SelectedSlot] == null)
        {
            Debug.LogWarning("Selected slot is null.");
            return;
        }
        // Checar ese slot tiene un item
        if (m_InvSlots[m_SelectedSlot].m_CurItem != null)
        {
            m_InvSlots[m_SelectedSlot].m_CurItem.UseItem(gameObject);
        }
        // Eliminar el item del inventario
        //m_InvSlots[m_SelectedSlot].m_CurItem = null;
        m_InvSlots.Remove(m_InvSlots[m_SelectedSlot]);
        // Actualizar Inventario
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

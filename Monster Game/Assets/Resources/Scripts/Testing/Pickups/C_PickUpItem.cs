using UnityEngine;

public class C_PickUpItem : C_PickUpProp
{
    public C_ItemSCOB m_ItemData;
    protected override void OnPyrPickup(C_PlayerMotor pyr)
    {
        Debug.Log(m_ItemData);
        if (pyr.gameObject.TryGetComponent(out C_InventoryManager inv) && inv.InventoryHasSpace())
        {
            inv.AddItem(this);
            base.OnPyrPickup(pyr);
        }
    }
}

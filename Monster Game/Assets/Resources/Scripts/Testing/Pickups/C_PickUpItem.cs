using UnityEngine;

public class C_PickUpItem : C_PickUpProp
{
    public C_ItemSCOB m_ItemData;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnPyrPickup(C_PlayerMotor pyr)
    {
        Debug.Log(m_ItemData);
        if (pyr.gameObject.TryGetComponent(out C_InventoryManager inv))
        {
            inv.AddItem(this);
            base.OnPyrPickup(pyr);
        }
    }
}

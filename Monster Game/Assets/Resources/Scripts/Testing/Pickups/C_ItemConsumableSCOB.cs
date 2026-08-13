using System;
using UnityEngine;
[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Items/ConsumableItem")]
public class C_ItemConsumableSCOB : C_ItemSCOB
{
    /* 0: Danio Recibido
     * 1: Miedo
     * 2: Estamina Usada
     * 3: Regeneracion de Estamina
     * 4: Porcentaje Bateria
     */
    public static Action<float[]> OnItemUse;
    public float[] m_StatToAffect = new float[5];
    public override void UseItem(GameObject User)
    {
        base.UseItem(User);
        OnItemUse?.Invoke(m_StatToAffect);
    }
}

using UnityEngine;
[CreateAssetMenu(fileName = "BasicItem", menuName = "Items/BasicItem")]
public class C_ItemSCOB: ScriptableObject
{
    string m_Name;
    public string m_ItemName;
    public Sprite m_Sprite;
    [TextArea] public string m_Description;
    public virtual void UseItem(GameObject User)
    {
        Debug.Log("Item " + m_Name + " used by: " + User);
    }
}

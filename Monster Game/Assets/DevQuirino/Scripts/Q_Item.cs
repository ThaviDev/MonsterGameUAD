using UnityEngine;

public class Q_Item : MonoBehaviour
{
    private uint ID = 0;
    public uint m_ID
    {
        get { return ID; }
        set { ID = value; }
    }


    private uint stackSize = 10;
    public uint m_stackSize
    {
        get { return stackSize; }
        set { stackSize = value; }
    }
}

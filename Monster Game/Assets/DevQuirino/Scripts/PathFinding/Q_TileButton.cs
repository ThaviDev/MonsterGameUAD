using UnityEngine;

public class Q_TileButton : MonoBehaviour
{
    [SerializeField] private TileType m_tileType;

    public TileType tileType
    {
        get => m_tileType; 
    }
}

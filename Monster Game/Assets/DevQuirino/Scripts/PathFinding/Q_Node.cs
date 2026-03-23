using UnityEngine;

public class Q_Node
{
    public Q_Node m_parent { get; set; }
    public Vector3Int m_position { get; set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Q_Node(Vector3Int _position)
    {
        this.m_position = _position;
    }
}

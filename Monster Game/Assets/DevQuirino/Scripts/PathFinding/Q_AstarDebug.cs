using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Q_AstarDebug : MonoBehaviour
{

    public static Q_AstarDebug instance { get; private set; }

    private List<GameObject> debugObjects = new List<GameObject>();

    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile tile;

    [SerializeField] private Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;

    [SerializeField] private Canvas canvas;

    [SerializeField] GameObject debugTextPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Hay mas de una instancia de Q_GameManager cuando es singleton");
        }
    }

    public void CraeteTiles(HashSet<Q_Node> openList, HashSet<Q_Node> closedList, Dictionary<Vector3Int, Q_Node> allNodes, Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach (Q_Node node in openList)
        {
            ColorTile(node.m_position, openColor);
        }
        foreach (Q_Node node in closedList)
        {
            ColorTile(node.m_position, closedColor);
        }

        if (path != null)
        {
            foreach(Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach (KeyValuePair<Vector3Int, Q_Node> node in allNodes)
        {
            if (node.Value.m_parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
                GenerateDebugtext(node.Value, go.GetComponent<Q_UIText>());
            }
        }
    }

    private void GenerateDebugtext(Q_Node node, Q_UIText debugText)
    {
        debugText.F.text = $"F:{node.F}";
        debugText.G.text = $"G:{node.G}";
        debugText.H.text = $"H:{node.H}";
        debugText.P.text = $"P:{node.m_position.x},{node.m_position.y}";



        if (node.m_parent.m_position.x < node.m_position.x && node.m_parent.m_position.y == node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.m_parent.m_position.x < node.m_position.x && node.m_parent.m_position.y > node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.m_parent.m_position.x < node.m_position.x && node.m_parent.m_position.y < node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 225));
        }
        else if (node.m_parent.m_position.x > node.m_position.x && node.m_parent.m_position.y == node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.m_parent.m_position.x > node.m_position.x && node.m_parent.m_position.y > node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.m_parent.m_position.x > node.m_position.x && node.m_parent.m_position.y < node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (node.m_parent.m_position.x == node.m_position.x && node.m_parent.m_position.y > node.m_position.y)
        {
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.m_parent.m_position.x == node.m_position.x && node.m_parent.m_position.y < node.m_position.y)
        { 
            debugText.m_Arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    public void ShowHide()
    {
        // Active/Deactive canvas
        canvas.gameObject.SetActive(!canvas.isActiveAndEnabled);

        // Invert tilemap alpha color
        Color _color = tilemap.color;
        _color.a = _color.a != 0 ? 0 :1;
        tilemap.color = _color;

    }

    public void QReset(Dictionary<Vector3Int, Q_Node> allNodes)
    {

        foreach (GameObject go in debugObjects)
        {
            Destroy(go);
        }

        debugObjects.Clear();

        // set all tiles to null
        foreach (Vector3Int pos in allNodes.Keys)
        {
            tilemap.SetTile(pos, null);
        }
    }

}

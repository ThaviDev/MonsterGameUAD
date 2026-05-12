using Pathfinding;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileType
{
    START = 0,
    GOAL,
    WATER,
    GRASS,
    PATH
}


public class Q_AStar : MonoBehaviour
{
    private TileType m_tileType;

    [SerializeField] private Tilemap m_tilemap;

    [SerializeField] private Tile[] m_tiles;

    [SerializeField] private RuleTile m_water; 

    [SerializeField] private Camera m_camera;

    [SerializeField] private LayerMask m_mask;

    private Vector3Int m_startPos, m_goalPos;

    private bool m_startIsSet, m_goalIsSet;

    private List<Vector3Int> m_waterTiles = new List<Vector3Int>();

    private HashSet<Q_Node> m_openList;

    private HashSet<Q_Node> m_closedList;

    private Stack<Vector3Int> m_path;

    private HashSet<Vector3Int> m_changedTiles = new HashSet<Vector3Int>();

    private Dictionary<Vector3Int, Q_Node> m_allNodes = new Dictionary<Vector3Int, Q_Node>();

    private Q_Node m_current;

    void Start()
    {
        
    }


    void Update()
    {
        // Detect mouse
        if (true == Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, m_mask);

            if (hit.collider != null)
            {
                Vector3 mouseWorldPos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = m_tilemap.WorldToCell(new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z));

                ChangeTile(clickPos);
            }
        }

        // Detect Input to run algorithm
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Algorithm();
            }
        }
    }

    public void Algorithm()
    {
        Debug.Log("Running Algoritm");
        if (null == m_current)
        {
            Initialize();
        }

        while (m_openList.Count > 0 && m_path == null) 
        {
            List<Q_Node> neighbors = FindNeighbors(m_current.m_position);

            ExamineNeighbors(neighbors, m_current);

            UpdateCurrentTile(ref m_current);

            m_path = GeneratePath(m_current);
        }
        

        // set tiles to path tiles 
        if (m_path != null)
        {
            foreach(Vector3Int pos in m_path)
            {
                if (pos != m_goalPos)
                {
                    m_tilemap.SetTile(pos, m_tiles[2]);
                }
            }
        }

        // debug
        var tileDebug = Q_AstarDebug.instance;
        tileDebug.CraeteTiles(m_openList, m_closedList, m_allNodes, m_startPos, m_goalPos, m_path);
    }



    private void Initialize()
    {
        m_current = getNode(m_startPos);

        m_openList = new HashSet<Q_Node>();

        m_closedList = new HashSet<Q_Node>();

        m_allNodes = new Dictionary<Vector3Int, Q_Node>();

        //Adding start to the opne list
        m_openList.Add(m_current);
    }

    private List<Q_Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Q_Node> neighbors = new List<Q_Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                if (y != 0 || x != 0)
                {
                    if (neighborPos != m_startPos && !m_waterTiles.Contains(neighborPos) && m_tilemap.GetTile(neighborPos)) // the tile exists
                    {
                        Q_Node neighbor = getNode(neighborPos);
                        neighbors.Add(neighbor);
                    }

                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<Q_Node> neighbors, Q_Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            Q_Node neighbor = neighbors[i]; 

            if (!ConectedDiagonally(current, neighbor))
            {
                continue;
            }


            int gScore = DetermineGScore(neighbors[i].m_position, current.m_position);

            if (m_openList.Contains(neighbor))
            {
                if (current.G + gScore < neighbor.G)// if this is a better path
                {
                    CalculateValues(current, neighbor, gScore); // Change parent
                }
            }
            else if (!m_closedList.Contains(neighbor))
            {

                CalculateValues(current, neighbor, gScore); // Change parent

                m_openList.Add(neighbors[i]);

            }
        }
    }

    private void CalculateValues(Q_Node parent, Q_Node neighbor, int cost)
    {
        neighbor.m_parent = parent;

        neighbor.G = parent.G + cost;
        neighbor.H = ((Mathf.Abs(neighbor.m_position.x - m_goalPos.x) + Mathf.Abs(neighbor.m_position.y - m_goalPos.y)) * 10);
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;
        int x  = current.x - neighbor.x;
        int y  = current.y - neighbor.y;

        if (Mathf.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Q_Node current)
    {
        m_openList.Remove(current);

        m_closedList.Add(current);

        if (m_openList.Count > 0)
        {
            current = m_openList.OrderBy(x => x.F).First(); //Lowest F Value
        }
    }

    private Q_Node getNode(Vector3Int _position)
    {
        if (m_allNodes.ContainsKey(_position))
        {
            return m_allNodes[_position];
        }
        else // if node doesnt exists create one
        {
            var node = new Q_Node(_position);
            m_allNodes.Add(_position, node);
            return node;
        }
    }

    public void ChangeTileType(Q_TileButton _button)
    {
        m_tileType = _button.tileType;
    }

    private void ChangeTile(Vector3Int _clickPos) // this need a lot of changes 
    {
        if (m_tileType == TileType.WATER)
        {
            m_tilemap.SetTile(_clickPos, m_water);
            m_waterTiles.Add(_clickPos);
        }
        else
        {
            if (m_tileType == TileType.START)
            {
                if(m_startIsSet)
                {
                    m_tilemap.SetTile(m_startPos, m_tiles[3]);
                }
                m_startIsSet = true;
                m_startPos = _clickPos;

            }
            else if (m_tileType == TileType.GOAL)
            {
                if (m_goalIsSet)
                {
                    m_tilemap.SetTile(m_goalPos, m_tiles[3]);
                }
                m_goalIsSet = true;
                m_goalPos = _clickPos;
            }

            m_tilemap.SetTile(_clickPos, m_tiles[(int)m_tileType]);

            m_changedTiles.Add(_clickPos);
        }
    }

    private bool ConectedDiagonally(Q_Node currentNode, Q_Node neighbor)
    {
        Vector3Int direct = currentNode.m_position - neighbor.m_position;
        Vector3Int first = new Vector3Int(m_current.m_position.x + (direct.x *-1), m_current.m_position.y, m_current.m_position.z);
        Vector3Int second = new Vector3Int(m_current.m_position.x, m_current.m_position.y + (direct.y * -1), m_current.m_position.z);

        if (m_waterTiles.Contains(first) || m_waterTiles.Contains(second))
        {
            return false;
        }
        return true;
    }

    private Stack<Vector3Int> GeneratePath(Q_Node current)
    {
        if (current.m_position == m_goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();
    
            while (current.m_position != m_startPos)
            {
                finalPath.Push(current.m_position);
    
                current = current.m_parent;
            }
    
            return finalPath;
        }
    
        return null;
    }

    public void QReset()
    {
        var tileDebug = Q_AstarDebug.instance;
        tileDebug.QReset(m_allNodes);


        //changed tiles now are grass
        foreach (Vector3Int pos in m_changedTiles)
        {
            m_tilemap.SetTile(pos, m_tiles[3]);
        }

        // path is grass now 
        foreach (Vector3Int pos in m_path)
        {
            m_tilemap.SetTile(pos, m_tiles[3]);
        }

        foreach (Vector3Int pos in m_waterTiles)
        {
            m_tilemap.SetTile(pos, m_tiles[3]);
        }


        //set start and goal to grass
        m_tilemap.SetTile(m_startPos, m_tiles[3]);
        m_tilemap.SetTile(m_goalPos, m_tiles[3]);

        m_startIsSet = false;
        m_goalIsSet = false;

        m_waterTiles.Clear();
        m_allNodes.Clear();
        m_path = null;
        m_current = null;
    }
}

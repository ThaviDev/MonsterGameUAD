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


public class C_AStar : MonoBehaviour
{
    private TileType m_tileType;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_floorTilemap;                    // main map used for world conversions and "floor" tiles
    [SerializeField] private Tilemap[] m_obstacleTilemaps;              // one or more maps containing walls/obstacles

    [Header("Tiles")]
    [SerializeField] private Tile[] m_tiles;

    [SerializeField] private RuleTile m_water;

    [SerializeField] private Camera m_camera;

    [SerializeField] private LayerMask m_mask;

    // Define qué tipos bloquean (puedes poner aquí todos los que quieras)
    [SerializeField] private TileType[] m_obstacleTypes = { TileType.WATER }; // Por defecto solo agua

    private Vector3Int m_startPos, m_goalPos;

    private bool m_startIsSet, m_goalIsSet;

    private List<Vector3Int> m_blockedTiles = new List<Vector3Int>();

    private HashSet<Q_Node> m_openList;

    private HashSet<Q_Node> m_closedList;

    private Stack<Vector3Int> m_path;

    private HashSet<Vector3Int> m_changedTiles = new HashSet<Vector3Int>();

    private Dictionary<Vector3Int, Q_Node> m_allNodes = new Dictionary<Vector3Int, Q_Node>();

    private Q_Node m_current;

    void Start()
    {
        // Build blocked list from obstacle tilemaps at start (if any)
        BuildBlockedTilesFromObstacleMaps();
    }


    void Update()
    {
        /*
        // Detect mouse
        if (true == Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(m_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, m_mask);

            if (hit.collider != null)
            {
                Vector3 mouseWorldPos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = (m_floorTilemap != null)
                    ? m_floorTilemap.WorldToCell(new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z))
                    : Vector3Int.FloorToInt(mouseWorldPos);

                ChangeTile(clickPos);
            }
        }*/

        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Algorithm();

            }
        }
    }

    private bool IsObstacle(TileType type)
    {
        return System.Array.Exists(m_obstacleTypes, t => t == type);
    }

    // Helper: returns true if there's a floor tile at this cell and not blocked by obstacle maps/list
    private bool IsWalkableCell(Vector3Int cell)
    {
        // must have a floor tile (so agents can stand on it)
        if (m_floorTilemap == null || m_floorTilemap.GetTile(cell) == null)
            return false;

        // not present in blocked list
        if (m_blockedTiles.Contains(cell))
            return false;

        return true;
    }

    // Build blocked list by scanning obstacle tilemaps (runs in Start)
    private void BuildBlockedTilesFromObstacleMaps()
    {
        m_blockedTiles.Clear();
        if (m_obstacleTilemaps == null || m_obstacleTilemaps.Length == 0)
            return;

        foreach (var obs in m_obstacleTilemaps)
        {
            if (obs == null) continue;
            BoundsInt b = obs.cellBounds;
            for (int x = b.xMin; x < b.xMax; x++)
            {
                for (int y = b.yMin; y < b.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, b.z);
                    if (obs.GetTile(pos) != null && !m_blockedTiles.Contains(pos))
                        m_blockedTiles.Add(pos);
                }
            }
        }
    }

    private Stack<Vector3Int> AStarSearch(Vector3Int startCell, Vector3Int goalCell)
    {
        // Verificar si inicio o fin son agua (o no transitables)
        if (!IsWalkableCell(startCell) || !IsWalkableCell(goalCell))
            return null;

        var openList = new HashSet<Q_Node>();
        var closedList = new HashSet<Q_Node>();
        var allNodes = new Dictionary<Vector3Int, Q_Node>();

        Q_Node startNode = GetOrCreateNode(startCell, allNodes);
        Q_Node goalNode = GetOrCreateNode(goalCell, allNodes);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Q_Node current = openList.OrderBy(x => x.F).First();
            openList.Remove(current);
            closedList.Add(current);

            // Meta alcanzada
            if (current.m_position == goalCell)
            {
                return BuildPath(current, startCell);
            }

            // Expandir vecinos
            List<Q_Node> neighbors = FindNeighbors(current.m_position, allNodes);
            foreach (Q_Node neighbor in neighbors)
            {
                if (!IsConnectedDiagonally(current, neighbor))
                    continue;

                int gCost = DetermineGScore(neighbor.m_position, current.m_position);

                if (openList.Contains(neighbor))
                {
                    if (current.G + gCost < neighbor.G)
                        UpdateNodeValues(current, neighbor, gCost, goalCell);
                }
                else if (!closedList.Contains(neighbor))
                {
                    UpdateNodeValues(current, neighbor, gCost, goalCell);
                    openList.Add(neighbor);
                }
            }
        }
        return null; // no se encontró camino
    }

    private Q_Node GetOrCreateNode(Vector3Int pos, Dictionary<Vector3Int, Q_Node> allNodes)
    {
        if (!allNodes.TryGetValue(pos, out Q_Node node))
        {
            node = new Q_Node(pos);
            allNodes[pos] = node;
        }
        return node;
    }

    private void UpdateNodeValues(Q_Node parent, Q_Node neighbor, int cost, Vector3Int goal)
    {
        neighbor.m_parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(neighbor.m_position.x - goal.x) + Mathf.Abs(neighbor.m_position.y - goal.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private Stack<Vector3Int> BuildPath(Q_Node goalNode, Vector3Int startCell)
    {
        Stack<Vector3Int> path = new Stack<Vector3Int>();
        Q_Node current = goalNode;
        while (current.m_position != startCell)
        {
            path.Push(current.m_position);
            current = current.m_parent;
        }
        return path;
    }

    // Updated neighbor finder: uses IsWalkableCell
    private List<Q_Node> FindNeighbors(Vector3Int parentPosition, Dictionary<Vector3Int, Q_Node> allNodes)
    {
        List<Q_Node> neighbors = new List<Q_Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector3Int nPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if (nPos != m_startPos && !m_blockedTiles.Contains(nPos) && IsWalkableCell(nPos))
                    neighbors.Add(GetOrCreateNode(nPos, allNodes));
            }
        }
        return neighbors;
    }

    private bool IsConnectedDiagonally(Q_Node currentNode, Q_Node neighbor)
    {
        Vector3Int direct = currentNode.m_position - neighbor.m_position;
        Vector3Int first = new Vector3Int(currentNode.m_position.x + (direct.x * -1), currentNode.m_position.y, currentNode.m_position.z);
        Vector3Int second = new Vector3Int(currentNode.m_position.x, currentNode.m_position.y + (direct.y * -1), currentNode.m_position.z);
        if (m_blockedTiles.Contains(first) || m_blockedTiles.Contains(second))
            return false;
        return true;
    }
    public void Algorithm()
    {
        Debug.Log("Running Algorithm");
        Stack<Vector3Int> path = AStarSearch(m_startPos, m_goalPos);
        if (path != null)
        {
            m_path = path;
            foreach (Vector3Int pos in m_path)
            {
                if (pos != m_goalPos)
                    m_floorTilemap.SetTile(pos, m_tiles[(int)TileType.PATH]); // use floor map for visual path
            }
        }
        // Debug visual
        // Necesitarás pasar las listas internas; la opción más limpia es refactorizar
        // Q_AstarDebug para aceptar los datos de la búsqueda. Por ahora puedes omitir.
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
                    if (neighborPos != m_startPos && !m_blockedTiles.Contains(neighborPos) && IsWalkableCell(neighborPos)) // the tile exists and not blocked
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
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Mathf.Abs(x - y) % 2 == 1)
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

    private void ChangeTile(Vector3Int _clickPos)
    {
        if (m_tileType == TileType.START || m_tileType == TileType.GOAL)
        {
            // Lógica para start y goal (sin cambios)
            if (m_tileType == TileType.START)
            {
                if (m_startIsSet) m_floorTilemap.SetTile(m_startPos, m_tiles[3]);
                m_startIsSet = true;
                m_startPos = _clickPos;
            }
            else
            {
                if (m_goalIsSet) m_floorTilemap.SetTile(m_goalPos, m_tiles[3]);
                m_goalIsSet = true;
                m_goalPos = _clickPos;
            }

            m_floorTilemap.SetTile(_clickPos, m_tiles[(int)m_tileType]);
            m_changedTiles.Add(_clickPos);
        }
        else if (IsObstacle(m_tileType))
        {
            // Put obstacle tile into the first obstacle tilemap (if configured)
            if (m_obstacleTilemaps != null && m_obstacleTilemaps.Length > 0 && m_obstacleTilemaps[0] != null)
            {
                m_obstacleTilemaps[0].SetTile(_clickPos, m_tiles[(int)m_tileType]);
                if (!m_blockedTiles.Contains(_clickPos))
                    m_blockedTiles.Add(_clickPos);
            }
            else
            {
                // fallback: mark in blocked list and paint on floor map
                m_floorTilemap.SetTile(_clickPos, m_tiles[(int)m_tileType]);
                if (!m_blockedTiles.Contains(_clickPos))
                    m_blockedTiles.Add(_clickPos);
            }
        }
        else
        {
            // Tile normal (GRASS, PATH...)
            m_floorTilemap.SetTile(_clickPos, m_tiles[(int)m_tileType]);
            m_changedTiles.Add(_clickPos);
        }
    }

    private bool ConectedDiagonally(Q_Node currentNode, Q_Node neighbor)
    {
        Vector3Int direct = currentNode.m_position - neighbor.m_position;
        Vector3Int first = new Vector3Int(m_current.m_position.x + (direct.x * -1), m_current.m_position.y, m_current.m_position.z);
        Vector3Int second = new Vector3Int(m_current.m_position.x, m_current.m_position.y + (direct.y * -1), m_current.m_position.z);

        if (m_blockedTiles.Contains(first) || m_blockedTiles.Contains(second))
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
            m_floorTilemap.SetTile(pos, m_tiles[3]);
        }

        // path is grass now 
        foreach (Vector3Int pos in m_path)
        {
            m_floorTilemap.SetTile(pos, m_tiles[3]);
        }

        foreach (Vector3Int pos in m_blockedTiles)
        {
            m_floorTilemap.SetTile(pos, m_tiles[3]);
        }


        //set start and goal to grass
        m_floorTilemap.SetTile(m_startPos, m_tiles[3]);
        m_floorTilemap.SetTile(m_goalPos, m_tiles[3]);

        m_startIsSet = false;
        m_goalIsSet = false;

        m_blockedTiles.Clear();
        m_allNodes.Clear();
        m_path = null;
        m_current = null;
    }
    public List<Vector3> GetPath(Vector3 worldStart, Vector3 worldGoal)
    {
        Vector3Int startCell = m_floorTilemap.WorldToCell(worldStart);
        Vector3Int goalCell = m_floorTilemap.WorldToCell(worldGoal);

        // Opcional: evitar que start/goal sean agua
        if (m_blockedTiles.Contains(startCell) || m_blockedTiles.Contains(goalCell))
            return null;

        Stack<Vector3Int> path = AStarSearch(startCell, goalCell);
        if (path == null || path.Count == 0)
            return null;

        // Convertir a posiciones mundo
        List<Vector3> worldPath = new List<Vector3>(path.Count);
        foreach (Vector3Int cell in path)
        {
            worldPath.Add(m_floorTilemap.GetCellCenterWorld(cell));
        }
        return worldPath;
    }

    // --- Added: provide path in world-space for agents to follow ---
    public List<Vector3> GetPathWorldPositions()
    {
        if (m_path == null || m_path.Count == 0)
            return null;

        // Stack.ToArray() returns an array in LIFO order (top -> bottom)
        // The stack was built by pushing from goal back to start; ToArray()
        // therefore returns nodes from nearest-to-start to goal.
        Vector3Int[] nodes = m_path.ToArray();
        List<Vector3> worldPositions = new List<Vector3>(nodes.Length);

        //for (int i = nodes.Length - 1; i >= 0; i--)
        //for (int i = 0; i < nodes.Length; i++)
        // Invertirlo por alguna razon que no se xd
        for (int i = 0; i < nodes.Length; i++)
        {
            // Use cell center so the agent moves to the tile center
            worldPositions.Add(m_floorTilemap.GetCellCenterWorld(nodes[i]));
        }

        return worldPositions;
    }

    public bool HasPath()
    {
        return m_path != null && m_path.Count > 0;
    }
}

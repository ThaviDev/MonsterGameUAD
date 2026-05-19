using System.Collections.Generic;
using UnityEngine;

public class C_PathToPathTest : MonoBehaviour
{
    [SerializeField] private Q_AStar m_aStar;
    [SerializeField] private Transform m_agent; // assign the object that should move (or leave null to use this transform)
    [SerializeField] private float m_speed = 3.0f;

    private List<Vector3> m_pathPositions;
    private int m_currentIndex;

    void Start()
    {
        if (m_aStar == null)
            m_aStar = FindObjectOfType<Q_AStar>();

        if (m_agent == null)
            m_agent = transform;

        if (m_aStar == null)
        {
            Debug.LogWarning("C_PathToPathTest: Q_AStar not found in scene.");
            return;
        }

        /*
        // Run the pathfinding (assumes start/goal already set in the tilemap)
        m_aStar.Algorithm();

        // Retrieve the path as world-space positions (tile centers)
        m_pathPositions = m_aStar.GetPathWorldPositions();
        m_currentIndex = 0;
        */

        if (m_pathPositions == null || m_pathPositions.Count == 0)
        {
            Debug.Log("C_PathToPathTest: No path returned from Q_AStar.");
        }
    }

    void Update()
    {
        // Detect Input to run algorithm
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_aStar.Algorithm();
            m_pathPositions = m_aStar.GetPathWorldPositions();
            m_currentIndex = 0;
        }

        if (m_pathPositions == null || m_pathPositions.Count == 0)
            return;

        if (m_currentIndex >= m_pathPositions.Count)
            return;

        print(m_pathPositions[m_currentIndex]);

        Vector3 target = m_pathPositions[m_currentIndex];
        Vector3 pos = m_agent.position;
        float step = m_speed * Time.deltaTime;
        m_agent.position = Vector3.MoveTowards(pos, target, step);

        // small threshold to advance to the next waypoint
        if (Vector3.Distance(m_agent.position, target) < 0.05f)
        {
            m_currentIndex++;
        }
    }
}

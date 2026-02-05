using Pathfinding;
using System.Collections;
using System.IO;
using UnityEngine;
public class C_CustomAIPath : MonoBehaviour
{
    public Transform m_targetPositon;

    private Seeker m_seeker;

    public Pathfinding.Path m_path;

    public float m_speed = 2;

    public float m_nextWaypointDistance = 3;

    private int m_currentWaypoint = 0;

    public bool m_reachedEndOfPath;

    void Start()
    {
        Seeker seeker = GetComponent<Seeker>();
        seeker.StartPath(transform.position, m_targetPositon.position, OnPathComplete);
    }

    public void OnPathComplete(Pathfinding.Path p)
    {
        Debug.Log("Yay, we got a  path back. Did it have an error? " + p.error);

        if (!p.error)
        {
            m_path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            m_currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (m_path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }
    }
}

using Pathfinding;
using UnityEngine;

public class TestAgentAStar : MonoBehaviour
{
    Seeker _seeker;
    AIPath _path;
    [SerializeField] Transform _target;
    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _path = GetComponent<AIPath>();
    }

    void Update()
    {
        _path.destination = _target.position;
    }
}

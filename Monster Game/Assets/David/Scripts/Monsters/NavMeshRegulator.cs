using UnityEngine;
using UnityEngine.AI;

public class NavMeshRegulator : MonoBehaviour
{
    [SerializeField] public Transform _target;
    private NavMeshAgent _agent;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }
    private void Update()
    {
        _agent.destination = _target.position;
    }
}

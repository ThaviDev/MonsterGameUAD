using UnityEngine;
using UnityEngine.AI;

public class NavMeshRegulator : MonoBehaviour
{
    [SerializeField] public Transform _target;
    NavMeshAgent _agent;
    bool _active = false;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        MstrApearDissapear.OnMonsterSpawn += Activate;
        _target = FindAnyObjectByType<PlayerMotor>().transform;
    }

    void Activate()
    {
        _active = true;
    }
    private void Update()
    {
        if (_active)
        {
            _agent.destination = _target.position;
        }
    }

    private void OnDestroy()
    {
        MstrApearDissapear.OnMonsterSpawn -= Activate;
    }
}

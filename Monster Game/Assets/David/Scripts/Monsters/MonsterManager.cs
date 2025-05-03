using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] FollowerEntity _myAI;
    [SerializeField] AIDestinationSetter _destinySet;
    Transform _target;
    bool _active = false;
    void Start()
    {
        _myAI.updateRotation = false;
        MstrApearDissapear.OnMonsterReady += Activate;
        _target = FindAnyObjectByType<PlayerMotor>().transform;
    }
    void Update()
    {

    }
    void Activate()
    {
        _destinySet.target = _target;
    }
}

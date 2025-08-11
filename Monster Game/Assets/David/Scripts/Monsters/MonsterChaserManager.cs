using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChaserManager : MonoBehaviour
{
    [SerializeField] FollowerEntity _myAI;
    [SerializeField] AIDestinationSetter _destinySet;
    [SerializeField] GameObject _pursuePointPrefab;
    [SerializeField] MChaser_Pursue _curPursuePoint;
    //Transform _target;
    bool _active = false;
    bool _lastActive = false;
    public bool SetActiveMonster { set { _active = value; } }
    void Start()
    {
        //_myAI.updateRotation = false;
        //_target = FindAnyObjectByType<PlayerMotor>().transform;
        _curPursuePoint = Instantiate(_pursuePointPrefab).GetComponent<MChaser_Pursue>();
        _curPursuePoint.SetChaserTransform = transform;
        _curPursuePoint.SetDestiny = _destinySet;
    }
    void Update()
    {
        if (_active != _lastActive)
        {
            _lastActive = _active;
            if (_active)
            {
                Activate();
            } else
            {
                Deactivate();
            }
        }
    }
    void Activate()
    {
        _curPursuePoint.SetMonsterIsActive = true;
        //_destinySet.target = _target;
    }

    void Deactivate() {
        _curPursuePoint.SetMonsterIsActive = false;
    }

    private void OnDestroy()
    {
        _curPursuePoint.SetIsAlive = false;
    }
}

using Pathfinding;
using UnityEngine;

public class MonsterBullManager : MonoBehaviour
{
    [SerializeField] FollowerEntity _myAI;
    [SerializeField] MonsterCircleTarget _circleLogic;
    [SerializeField] bool _canCircle;
    [SerializeField] GameObject _target;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float _rayDistance = 5f;

    [SerializeField] float _prepareChargeTime = 10f;
    [SerializeField] float _waitToAttack;
    [SerializeField] Transform _aimCharge;

    void OnEnable()
    {
        _myAI.updateRotation = false;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (_canCircle)
        {
            _circleLogic.CircleTarget(_target.transform);
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (_target.transform.position - transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, _rayDistance, _obstacleLayer | _playerLayer);
        Debug.DrawRay(transform.position, direction * _rayDistance, Color.red);

        if (ray.collider != null)
        {
            if (((1 << ray.collider.gameObject.layer) & _playerLayer) != 0)
            {
                print("Choco con el jugador: " + ray.collider.name);
            }
            else if (((1 << ray.collider.gameObject.layer) & _obstacleLayer) != 0)
            {
                print("Choco con obstaculo: " + ray.collider.name);
            }
        } else
        {
            print("No choco con nada");
        }
    }
}

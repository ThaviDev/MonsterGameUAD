using Pathfinding;
using UnityEngine;

public class MonsterBullManager : MonoBehaviour
{
    [SerializeField] FollowerEntity _myAI;
    [SerializeField] MonsterCircleTarget _circleLogic;
    [SerializeField] Transform _aimCharge;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] GameObject _target;
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] SpriteRenderer _sprt;
    [SerializeField] Color _normalCol;
    [SerializeField] Color _chargeCol;
    [SerializeField] float _rayDistance = 5f;

    [SerializeField] float _setPrepareChargeTime = 10f;
    float _prepareChargeTime = 10f;
    [SerializeField] float _setWaitToAttack = 0.5f;
    float _waitToAttack = 0.5f;
    [SerializeField] float _chargeSpeed;
    bool _hasCharged;
    [SerializeField] float _setChargeDuration = 3f;
    float _chargeDuration = 3f;

    void OnEnable()
    {
        _myAI.updateRotation = false;
    }
    void Start()
    {
        _target = FindAnyObjectByType<PlayerMotor>().gameObject;
        RestartChargeValues();
    }

    void Update()
    {
        print("Valores>> Preparado para embestida: " + _prepareChargeTime + ", Duracion de carga: " + _chargeDuration
            + ", Tiempo de espera: " + _waitToAttack);
        if (_prepareChargeTime > 0)
        {
            _circleLogic.CircleTarget(_target.transform);
        } else
        {
            _sprt.color = _chargeCol;
            _myAI.isStopped = true;
            _aimCharge = _target.transform;
            _waitToAttack -= Time.deltaTime;
        }
        if (_waitToAttack < 0) {
            Charge();
        }
    }

    void RestartChargeValues()
    {
        _sprt.color = _normalCol;
        _hasCharged = false;
        _myAI.isStopped = false;
        _prepareChargeTime = _setPrepareChargeTime;
        _chargeDuration = _setChargeDuration;
        _waitToAttack = _setWaitToAttack;
        _rb.linearVelocity = new Vector3(0, 0, 0);
    }

    void Charge()
    {
        if (!_hasCharged)
        {
            print("Hago una carga");
            Vector2 direction = (_aimCharge.position - transform.position).normalized;
            _rb.AddForce(direction * _chargeSpeed, ForceMode2D.Impulse);
            _hasCharged = true;
        } else if (_chargeDuration > 0)
        {
            _chargeDuration -= Time.deltaTime;
        } else
        {
            RestartChargeValues();
        }
    }
    private void FixedUpdate()
    {
        RayCastCheck();
    }

    void RayCastCheck()
    {
        Vector2 direction = (_target.transform.position - transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, _rayDistance, _obstacleLayer | _playerLayer);
        Debug.DrawRay(transform.position, direction * _rayDistance, Color.red);

        if (ray.collider != null)
        {
            if (((1 << ray.collider.gameObject.layer) & _playerLayer) != 0)
            {
                print("Choco con el jugador: " + ray.collider.name);
                _prepareChargeTime -= Time.deltaTime;
            }
            else if (((1 << ray.collider.gameObject.layer) & _obstacleLayer) != 0)
            {
                print("Choco con obstaculo: " + ray.collider.name);
            }
        }
        else
        {
            print("No choco con nada");
        }
    }
}

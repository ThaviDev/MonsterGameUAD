using Pathfinding;
using UnityEngine;

public class MChaser_Pursue : MonoBehaviour
{
    // Crear un prediction point en base al del jugador
    // Igualar ese prediction point al target
    // Hacer la lógica de que el target se acerce al jugador en cierto rango (checa si necesita time to arrive)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerPredictionPoint _pyrPredPoint; // Accede a su posicion y su arrive time
    private Transform _mChaserTrans;
    private AIDestinationSetter _destinySet;
    private bool _monsterIsActive;
    private bool _monsterIsAlive;
    public Transform SetChaserTransform { set { _mChaserTrans = value; } }
    public AIDestinationSetter SetDestiny { set { _destinySet = value; } }
    public bool SetMonsterIsActive { set { _monsterIsActive = value; } }
    public bool SetIsAlive { set { _monsterIsAlive = value; } }

    void Start()
    {
        _pyrPredPoint = FindAnyObjectByType<PlayerPredictionPoint>();
        _destinySet.target = transform;
        _monsterIsActive = false;
        _monsterIsAlive = true;
    }
    void Update()
    {
        if (!_monsterIsAlive)
        {
            MonsterIsDead();
        }
        if (_monsterIsActive)
        {
            Pursue();
        } else if (_monsterIsAlive)
        {
            transform.position = _mChaserTrans.position;
        }

    }

    private void MonsterIsDead()
    {
        Destroy(gameObject);
    }

    private void Pursue()
    {
        float _arriveTime = _pyrPredPoint.GetArriveTime;
        float _speed = _pyrPredPoint.GetSpeed;
        Vector2 _direction = _pyrPredPoint.GetDirection;
        Transform _playerPos = _pyrPredPoint.GetPyrTrans;

        float _predicRadious = _speed * _arriveTime;

        if (_predicRadious < (_mChaserTrans.position - _playerPos.position).magnitude)
        {
            transform.position = _pyrPredPoint.transform.position;
        }
        else
        {
            transform.position =
                new Vector2(_playerPos.transform.position.x, _playerPos.transform.position.y)
                + (_direction * (_mChaserTrans.position - _playerPos.position).magnitude);
        }
    }
}

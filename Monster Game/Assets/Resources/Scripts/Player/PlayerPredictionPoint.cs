using UnityEngine;

public class PlayerPredictionPoint : MonoBehaviour
{
    [SerializeField] float _arriveTime;
    [SerializeField] PlayerStadistics _pStats; // para sacar speed y direction
    [SerializeField] Transform _pyrTrans;
    Vector2 _dir;
    float _speed;

    public float GetArriveTime { get { return _arriveTime; } }
    public Vector2 GetDirection { get { return _dir; } }
    public float GetSpeed { get { return _speed; } }
    public Transform GetPyrTrans { get { return _pyrTrans; } }
    void Start()
    {

    }
    void Update()
    {
        _dir = PlayerInputs.Instance.MovementVector.normalized;
        _speed = _pStats.GetCurrentMaxSpeed;
        transform.position = new Vector2(_pyrTrans.position.x,_pyrTrans.position.y) + (_dir * _speed * _arriveTime);
    }
}

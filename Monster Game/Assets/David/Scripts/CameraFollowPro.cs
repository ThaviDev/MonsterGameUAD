using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPro : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] Transform _target;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _yOffset;
    [SerializeField] float _softZoneX;
    [SerializeField] float _softZoneY;
    [SerializeField] float _deadZoneX;
    [SerializeField] float _deadZoneY;
    [SerializeField] float _followSpeed;

    float _xSpeed;
    float _ySpeed;
    void Start()
    {

    }

    void Update()
    {
        var targetPosY = _target.position.y + _yOffset;
        //var ySpeed;
        var targetCamDistanceDiferenceX = _cam.transform.position.x - _target.position.x;
        var targetCamDistanceDiferenceY = _cam.transform.position.y - targetPosY;

        //Debug.Log(_target.position.x - (_softZoneX + _cam.transform.position.x));
        var positiveDistanceFromSoftZoneX = _target.position.x - (_softZoneX + _cam.transform.position.x);
        var negativeDistanceFromSoftZoneX = _target.position.x - (-_softZoneX + _cam.transform.position.x);
        var positiveDistanceFromSoftZoneY = targetPosY - (_softZoneY + _cam.transform.position.y);
        var negativeDistanceFromSoftZoneY = targetPosY - (-_softZoneY + _cam.transform.position.y);

        if (targetCamDistanceDiferenceX > _softZoneX)
        {
            _xSpeed = -_followSpeed * -negativeDistanceFromSoftZoneX;
        }
        else if (targetCamDistanceDiferenceX < -_softZoneX)
        {
            _xSpeed = _followSpeed * positiveDistanceFromSoftZoneX;
        }
        else
        {
            _xSpeed = 0;
        }
        if (targetCamDistanceDiferenceY > _softZoneY)
        {
            _ySpeed = -_followSpeed * -negativeDistanceFromSoftZoneY;
        }
        else if (targetCamDistanceDiferenceY < -_softZoneY)
        {
            _ySpeed = _followSpeed * positiveDistanceFromSoftZoneY;
        }
        else
        {
            _ySpeed = 0;
        }
        _rb.linearVelocity = new Vector3(_xSpeed, _ySpeed);

        var positiveDistanceFromDeadZoneX = _target.position.x - (_deadZoneX + _cam.transform.position.x);
        var negativeDistanceFromDeadZoneX = _target.position.x - (-_deadZoneX + _cam.transform.position.x);
        var positiveDistanceFromDeadZoneY = targetPosY - (_deadZoneY + _cam.transform.position.y);
        var negativeDistanceFromDeadZoneY = targetPosY - (-_deadZoneY + _cam.transform.position.y);
        if (targetCamDistanceDiferenceX > _deadZoneX)
        {
            transform.position += new Vector3(negativeDistanceFromDeadZoneX, 0f, 0f);
        }
        if (targetCamDistanceDiferenceX < -_deadZoneX)
        {
            transform.position += new Vector3(positiveDistanceFromDeadZoneX, 0f, 0f);
        }
        if (targetCamDistanceDiferenceY > _deadZoneY)
        {
            transform.position += new Vector3(0f, negativeDistanceFromDeadZoneY, 0f);
        }
        if (targetCamDistanceDiferenceY < -_deadZoneY)
        {
            transform.position += new Vector3(0f, positiveDistanceFromDeadZoneY, 0f);
        }
    }
}

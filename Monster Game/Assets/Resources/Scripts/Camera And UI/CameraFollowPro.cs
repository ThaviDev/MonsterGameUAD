using System.Collections;
using UnityEngine;

public class CameraFollowPro : MonoBehaviour
{
    [Header("Configuración Básica")]
    [SerializeField] Camera _cam;
    [SerializeField] Transform _target;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _yOffset;

    [Header("Zonas de Seguimiento")]
    [SerializeField] float _softZoneX;
    [SerializeField] float _softZoneY;
    [SerializeField] float _deadZoneX;
    [SerializeField] float _deadZoneY;

    [Header("Velocidad y Overshoot")]
    [SerializeField] float _followSpeed;
    [SerializeField] float _overshootIntensity = 0.5f; // Fuerza del overshoot
    [SerializeField] float _velocityOvershootFactor = 0.1f; // Influencia de la velocidad
    [SerializeField] float _maxOvershoot = 3f; // Límite máximo de desplazamiento

    [Header("Camera Shake")]
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] float _shakeMagnitude = 0.5f;

    float _xSpeed;
    float _ySpeed;
    Vector2 _lastCamVelocity;

    // Shake internals
    Coroutine _shakeCoroutine;
    Vector3 _lastShakeOffset = Vector3.zero;

    private void Start()
    {
        C_PlayerMotor.OnPyrHit += PlayerHitEvent;
    }

    void Update()
    {
        Vector2 overshoot = CalculateOvershoot();
        Vector2 targetPosition = (Vector2)_target.position + new Vector2(0, _yOffset) + overshoot;

        HandleCameraMovement(targetPosition);
        ApplyDeadZones(targetPosition);
    }

    void PlayerHitEvent(Collider2D collider, float damage, float fear, float stunDuration, float knockbackForce)
    {
        TriggerScreenShake(_shakeDuration, _shakeMagnitude);
    }

    Vector2 CalculateOvershoot()
    {
        // Calcular dirección del movimiento basado en velocidad de la cámara
        Vector2 moveDirection = _rb.linearVelocity.normalized;

        // Calcular magnitud del overshoot (base + influencia de velocidad)
        float velocityMagnitude = Mathf.Clamp(_rb.linearVelocity.magnitude * _velocityOvershootFactor, 0, _maxOvershoot);
        float overshootAmount = _overshootIntensity * velocityMagnitude;

        return moveDirection * overshootAmount;
    }

    void HandleCameraMovement(Vector2 targetPosition)
    {
        float targetCamDistanceX = _cam.transform.position.x - targetPosition.x;
        float targetCamDistanceY = _cam.transform.position.y - targetPosition.y;

        // Calcular distancias a las zonas suaves
        float positiveDistanceFromSoftZoneX = targetPosition.x - (_softZoneX + _cam.transform.position.x);
        float negativeDistanceFromSoftZoneX = targetPosition.x - (-_softZoneX + _cam.transform.position.x);
        float positiveDistanceFromSoftZoneY = targetPosition.y - (_softZoneY + _cam.transform.position.y);
        float negativeDistanceFromSoftZoneY = targetPosition.y - (-_softZoneY + _cam.transform.position.y);

        // Manejar movimiento en X
        if (targetCamDistanceX > _softZoneX)
        {
            _xSpeed = -_followSpeed * -negativeDistanceFromSoftZoneX;
        }
        else if (targetCamDistanceX < -_softZoneX)
        {
            _xSpeed = _followSpeed * positiveDistanceFromSoftZoneX;
        }
        else
        {
            _xSpeed = 0;
        }

        // Manejar movimiento en Y
        if (targetCamDistanceY > _softZoneY)
        {
            _ySpeed = -_followSpeed * -negativeDistanceFromSoftZoneY;
        }
        else if (targetCamDistanceY < -_softZoneY)
        {
            _ySpeed = _followSpeed * positiveDistanceFromSoftZoneY;
        }
        else
        {
            _ySpeed = 0;
        }

        // Aplicar velocidad
        _rb.linearVelocity = new Vector2(_xSpeed, _ySpeed);
        _lastCamVelocity = _rb.linearVelocity;
    }

    void ApplyDeadZones(Vector2 targetPosition)
    {
        float targetCamDistanceX = _cam.transform.position.x - targetPosition.x;
        float targetCamDistanceY = _cam.transform.position.y - targetPosition.y;

        // Calcular distancias a las zonas muertas
        float positiveDistanceFromDeadZoneX = targetPosition.x - (_deadZoneX + _cam.transform.position.x);
        float negativeDistanceFromDeadZoneX = targetPosition.x - (-_deadZoneX + _cam.transform.position.x);
        float positiveDistanceFromDeadZoneY = targetPosition.y - (_deadZoneY + _cam.transform.position.y);
        float negativeDistanceFromDeadZoneY = targetPosition.y - (-_deadZoneY + _cam.transform.position.y);

        // Aplicar zonas muertas
        if (targetCamDistanceX > _deadZoneX)
        {
            transform.position += new Vector3(negativeDistanceFromDeadZoneX, 0f, 0f);
        }
        if (targetCamDistanceX < -_deadZoneX)
        {
            transform.position += new Vector3(positiveDistanceFromDeadZoneX, 0f, 0f);
        }
        if (targetCamDistanceY > _deadZoneY)
        {
            transform.position += new Vector3(0f, negativeDistanceFromDeadZoneY, 0f);
        }
        if (targetCamDistanceY < -_deadZoneY)
        {
            transform.position += new Vector3(0f, positiveDistanceFromDeadZoneY, 0f);
        }
    }

    public void TriggerScreenShake(float duration, float magnitude)
    {
        // Actualizar parámetros
        _shakeDuration = duration;
        _shakeMagnitude = magnitude;

        // Si ya hay una corutina de shake corriendo, detenerla para reiniciar
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            // Asegurar que se limpie cualquier offset pendiente
            if (_lastShakeOffset != Vector3.zero)
            {
                transform.position -= _lastShakeOffset;
                _lastShakeOffset = Vector3.zero;
            }
            _shakeCoroutine = null;
        }

        _shakeCoroutine = StartCoroutine(ScreenShakeCoroutine());
    }

    IEnumerator ScreenShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            // Progreso 0..1
            float progress = elapsed / _shakeDuration;
            // Damper para disminuir intensidad con el tiempo (suavizado)
            float damper = 1f - Mathf.Clamp01(progress);

            // Generar offset en X/Y (2D) usando Random.insideUnitCircle para variación natural
            Vector2 offset2D = Random.insideUnitCircle * (_shakeMagnitude * damper);
            Vector3 newOffset = new Vector3(offset2D.x, offset2D.y, 0f);

            // Aplicar nuevo offset en relación al offset anterior (evita drift cuando la cámara se mueve)
            transform.position -= _lastShakeOffset;
            transform.position += newOffset;
            _lastShakeOffset = newOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Limpiar offset final
        if (_lastShakeOffset != Vector3.zero)
        {
            transform.position -= _lastShakeOffset;
            _lastShakeOffset = Vector3.zero;
        }

        _shakeCoroutine = null;
    }

}

/*
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
*/

using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private bool _is2D = true;
    [SerializeField] private bool _useMouseRotation = false; // Nuevo flag para alternar entre joystick/mouse

    private Vector2 _inputDirection;

    private void Update()
    {
        if (_useMouseRotation)
        {
            RotateTowardsMouse();
        }
        else
        {
            RotateWithJoystick();
        }
    }

    // Rotación con joystick (lógica original)
    private void RotateWithJoystick()
    {
        _inputDirection = PlayerInputs.OnAimChange();
        if (_inputDirection != Vector2.zero)
        {
            ApplyRotation(CalculateTargetRotation(_inputDirection));
        }
    }

    // Rotación hacia el puntero del mouse (nueva lógica)
    private void RotateTowardsMouse()
    {
        Vector2 mouseDirection = GetMouseDirection();
        if (mouseDirection != Vector2.zero)
        {
            ApplyRotation(CalculateTargetRotation(mouseDirection));
        }
    }

    // Obtiene la dirección del mouse relativa al objeto
    private Vector2 GetMouseDirection()
    {
        if (_is2D)
        {
            // Para 2D: mouse en coordenadas del mundo
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return (mouseWorldPos - transform.position).normalized;
        }
        else
        {
            // Para 3D: mouse en el plano X-Z (top-down)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position.y);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                Vector3 direction3D = (hitPoint - transform.position).normalized;
                return new Vector2(direction3D.x, direction3D.z); // X y Z como Vector2
            }
            return Vector2.zero;
        }
    }

    // Calcula la rotación objetivo
    private Quaternion CalculateTargetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_is2D)
        {
            return Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            return Quaternion.Euler(0, angle, 0);
        }
    }

    // Aplica la rotación suavizada
    private void ApplyRotation(Quaternion targetRotation)
    {
        if (_rotationSpeed <= 0)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
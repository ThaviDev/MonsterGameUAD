using UnityEngine;

public class FlipSpriteMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] bool _startLeft = true;
    void Update()
    {
        if (_rb?.linearVelocity.x > 0)
        {
            if (_startLeft)
            {
                _renderer.flipX = true;
            }
            else
            {
                _renderer.flipX = false;
            }
        } else
        {
            if (_startLeft)
            {
                _renderer.flipX = false;
            }
            else
            {
                _renderer.flipX = true;
            }
        }
    }
}

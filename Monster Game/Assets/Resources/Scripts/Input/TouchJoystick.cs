using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform _circleSpace;
    public RectTransform _innerCircle;
    private bool _canTouchedToSpawn;
    [SerializeField] Canvas _canvas;

    [SerializeField][Range(0.1f, 0.9f)] private float screenSplitRatio = 0.5f; // 0.5 = mitad derecha

    static public Vector2 inputVector;
    public void Update()
    {
        if (_canTouchedToSpawn && Input.touchCount >= 1)
        {
            // Buscar el primer toque que esté en la fase "Began" y en el área izquierda
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && IsTouchOnLeftSide(touch))
                {
                    _canTouchedToSpawn = false;
                    MoveJoyStickPos(touch);
                    break;
                }
            }
        }
        if (Input.touchCount <= 0)
        {
            _canTouchedToSpawn = true;
        }
    }
    private bool IsTouchOnLeftSide(Touch touch)
    {
        float screenWidth = Screen.width;
        float splitWidth = screenWidth * screenSplitRatio;
        return touch.position.x < splitWidth;
    }
    private void MoveJoyStickPos(Touch touch)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            touch.position,
            _canvas.worldCamera,
            out Vector2 localPoint);

        _circleSpace.anchoredPosition = localPoint;
    }
    public void OnDrag(PointerEventData eventData)
    {
        CalculateInnerCirclePosition(eventData.position);
        CalculateInputVector();
        CalculateInnerCircleRotation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _innerCircle.anchoredPosition = Vector2.zero;
        _innerCircle.localRotation = Quaternion.identity;
        inputVector = Vector2.zero;
    }

    private void CalculateInnerCirclePosition(Vector2 position)
    {
        Vector2 directPosition = position - (Vector2)_circleSpace.position;
        if (directPosition.magnitude > _circleSpace.rect.width / 2f)
            directPosition = directPosition.normalized * _circleSpace.rect.width / 2f;
        _innerCircle.anchoredPosition = directPosition;
    }

    private void CalculateInputVector()
    {
        inputVector = _innerCircle.anchoredPosition / (_circleSpace.rect.size / 2f);
    }

    private void CalculateInnerCircleRotation()
    {
        _innerCircle.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, inputVector));
    }
}
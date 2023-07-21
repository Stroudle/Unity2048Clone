using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public delegate void SwipeInput(Vector2Int input);
    public static event SwipeInput OnSwipeInput;

    [SerializeField]
    private float _minimumDistance = .2f;
    [SerializeField]
    private float _maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float _directionThreshold = .9f;

    private InputHandler _inputHandler;

    private Vector2 _startPosition;
    private float _startTime;
    private Vector2 _endPosition;
    private float _endTime;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        _inputHandler.OnStartTouch += SwipeStart;
        _inputHandler.OnCancelTouch += SwipeCancel;
    }

    private void OnDisable()
    {
        _inputHandler.OnStartTouch -= SwipeStart;
        _inputHandler.OnCancelTouch -= SwipeCancel;
    }

    private void SwipeStart(Vector2 position, float duration)
    {
        _startPosition = position;
        _startTime = duration;
    }

    private void SwipeCancel(Vector2 position, float duration)
    {
        _endPosition = position;
        _endTime = duration;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        float swipeDistance = Vector3.Distance(_startPosition, _endPosition);
        float swipeDuration = _endTime - _startTime;

        if(swipeDistance >= _minimumDistance && swipeDuration <= _maximumTime)
        {
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5f);
            Vector3 direction = _endPosition - _startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > _directionThreshold)
        {
            OnSwipeInput?.Invoke(Vector2Int.up);
        }
        else if(Vector2.Dot(Vector2.down, direction) > _directionThreshold)
        {
            OnSwipeInput?.Invoke(Vector2Int.down);
        }
        else if(Vector2.Dot(Vector2.left, direction) > _directionThreshold)
        {
            OnSwipeInput?.Invoke(Vector2Int.left);
        }
        else if(Vector2.Dot(Vector2.right, direction) > _directionThreshold)
        {
            OnSwipeInput?.Invoke(Vector2Int.right);
        }
    }
}
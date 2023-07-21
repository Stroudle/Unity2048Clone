using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public delegate void KeyboardInput(Vector2Int input);
    public static event KeyboardInput OnKeyboardInput;

    public delegate void TouchInput(Vector2 position, float duration);
    public event TouchInput OnStartTouch;
    public event TouchInput OnCancelTouch;

    private PlayerInput _playerInput;
    private InputAction _keyboardInput;
    private InputAction _touchContact;
    private InputAction _touchPosition;
    private Camera _mainCamera;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _keyboardInput = _playerInput.actions["KeyboardInput"];
        _touchContact = _playerInput.actions["TouchContact"];
        _touchPosition = _playerInput.actions["TouchPosition"];
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _keyboardInput.performed += OnKeyboardInputPerformed;
        _touchContact.started += OnStartTouchPrimary;
        _touchContact.canceled += OnCancelTouchPrimary;
    }

    private void OnDisable() 
    {
        _keyboardInput.performed -= OnKeyboardInputPerformed;
        _touchContact.started -= OnStartTouchPrimary;
        _touchContact.canceled -= OnCancelTouchPrimary;
    }

    private void OnStartTouchPrimary(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _touchPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    private void OnCancelTouchPrimary(InputAction.CallbackContext context)
    {   
        OnCancelTouch?.Invoke(Utils.ScreenToWorld(_mainCamera, _touchPosition.ReadValue<Vector2>()), (float)context.time);
    }

    private void OnKeyboardInputPerformed(InputAction.CallbackContext context)
    {
        Vector2Int input = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        OnKeyboardInput?.Invoke(input.x != 0 ? new Vector2Int(input.x, 0) : new Vector2Int(0, input.y));
    }
}
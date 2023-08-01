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
    private InputAction _keyboardHorizontal;
    private InputAction _keyboardVertical;
    private InputAction _touchContact;
    private InputAction _touchPosition;
    private Camera _mainCamera;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _keyboardHorizontal = _playerInput.actions["KeyboardHorizontal"];
        _keyboardVertical = _playerInput.actions["KeyboardVertical"];
        _touchContact = _playerInput.actions["TouchContact"];
        _touchPosition = _playerInput.actions["TouchPosition"];
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _keyboardHorizontal.performed += OnKeyboardInputHorizontal;
        _keyboardVertical.performed += OnKeyboardInputVertical;
        _touchContact.started += OnStartTouchPrimary;
        _touchContact.canceled += OnCancelTouchPrimary;
    }

    private void OnDisable() 
    {
        _keyboardHorizontal.performed -= OnKeyboardInputHorizontal;
        _keyboardVertical.performed -= OnKeyboardInputVertical;
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

    private void OnKeyboardInputHorizontal(InputAction.CallbackContext context)
    {
        Vector2Int input = new((int)context.ReadValue<float>(), 0);
        OnKeyboardInput?.Invoke(input);
    }

    private void OnKeyboardInputVertical(InputAction.CallbackContext context)
    {
        Vector2Int input = new(0, (int)context.ReadValue<float>());
        OnKeyboardInput?.Invoke(input);
    }
}
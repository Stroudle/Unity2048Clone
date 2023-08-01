using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public delegate void KeyboardInput(Vector2Int input);
    public static event KeyboardInput OnKeyboardInput;

    private PlayerInput _playerInput;
    private InputAction _keyboardHorizontal;
    private InputAction _keyboardVertical;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _keyboardHorizontal = _playerInput.actions["KeyboardHorizontal"];
        _keyboardVertical = _playerInput.actions["KeyboardVertical"];
    }

    private void OnEnable()
    {
        _keyboardHorizontal.performed += OnKeyboardInputHorizontal;
        _keyboardVertical.performed += OnKeyboardInputVertical;
    }

    private void OnDisable() 
    {
        _keyboardHorizontal.performed -= OnKeyboardInputHorizontal;
        _keyboardVertical.performed -= OnKeyboardInputVertical;
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
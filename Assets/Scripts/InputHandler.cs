using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference movement;

    public delegate void Movement(Vector2Int input);
    public static event Movement OnMovementInput;

    private void OnEnable()
    {
        movement.action.performed += OnMovementActionPerformed;
    }

    private void OnDisable() 
    {
        movement.action.performed -= OnMovementActionPerformed;
    }

    private void OnMovementActionPerformed(InputAction.CallbackContext callback)
    {
        var input = Vector2Int.CeilToInt(callback.ReadValue<Vector2>());
        var direction = input.x > 0 ? Vector2Int.right : (input.x < 0 ? Vector2Int.left : (input.y > 0 ? Vector2Int.up : Vector2Int.down));

        Debug.Log(input);
        Debug.Log(direction);
        //OnMovementInput.Invoke(myinput);
    }
}
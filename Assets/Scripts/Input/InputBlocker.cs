using UnityEngine;
using UnityEngine.InputSystem;

public class InputBlocker : MonoBehaviour
{
    public static InputBlocker Instance { get; private set; }

    private static PlayerInput _playerInput;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _playerInput = GetComponent<PlayerInput>();
    }

    public static void BlockInputs()
    {
        _playerInput.enabled = false;
    }

    public static void UnblockInputs()
    {
        _playerInput.enabled = true;
    }
}

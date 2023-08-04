using System;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [SerializeField]
    private ColorScheme[] _colors;

    public ColorScheme GetColor(int value)
    {
        int index = (int) Mathf.Log(value, 2) - 1;
        return _colors[Mathf.Clamp(index, 0, _colors.Length)];
    }

    public ColorScheme NextColor(ColorScheme current)
    {
        int index = Array.IndexOf(_colors, current) + 1;
        return (index >= _colors.Length) ? _colors[0] : _colors[index];
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}

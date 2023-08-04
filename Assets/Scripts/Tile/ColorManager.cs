using System;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [SerializeField]
    private ColorScheme[] _tileColors;

    public ColorScheme GetColor(int value)
    {
        int index = (int) Mathf.Log(value, 2) - 1;
        return _tileColors[Mathf.Clamp(index, 0, _tileColors.Length)];
    }

    public ColorScheme GetNextColor(ColorScheme tileColor)
    {
        int index = GetIndexOf(tileColor) + 1;
        return (index >= _tileColors.Length) ? _tileColors[0] : _tileColors[index];
    }

    private int GetIndexOf(ColorScheme element)
    {
        return Array.IndexOf(_tileColors, element);
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

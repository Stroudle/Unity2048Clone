using System;
using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    public static TileColorManager Instance { get; private set; }

    [SerializeField]
    private TileColorData[] _tileColors;

    public TileColorData GetColor(int value)
    {
        int index = (int) Mathf.Log(value, 2) - 1;
        return _tileColors[Mathf.Clamp(index, 0, _tileColors.Length)];
    }

    public TileColorData GetNextColor(TileColorData tileColor)
    {
        int index = GetIndexOf(tileColor) + 1;
        return (index >= _tileColors.Length) ? _tileColors[0] : _tileColors[index];
    }

    private int GetIndexOf(TileColorData element)
    {
        return Array.IndexOf(_tileColors, element);
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

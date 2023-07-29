using System;
using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    public static TileColorManager Instance { get; private set; }

    private TileColorData[] _tileColors;

    public TileColorData GetColor(int index)
    {
        return _tileColors[index];
    }

    public TileColorData GetNextColor(TileColorData tileColor)
    {
        int index = Mathf.Clamp(GetIndexOf(tileColor) + 1, 0, _tileColors.Length - 1);
        return _tileColors[index];
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

        LoadData();
    }

    private void LoadData()
    {
        _tileColors = Resources.LoadAll<TileColorData>("TileColorData");
    }
}

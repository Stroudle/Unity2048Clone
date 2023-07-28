using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    public static TileColorManager Instance { get; private set; }

    private TileColorData[] _tileColorsData;

    public Color GetBackgroundColor(int value)
    {
        return _tileColorsData[GetIndex(value)].backgroundColor;
    }

    public Color GetTextColor(int value)
    {
        return _tileColorsData[GetIndex(value)].textColor;
    }

    private int GetIndex(int value)
    {
        int index = (int)Mathf.Log(value, 2) - 1;
        if(index >= _tileColorsData.Length)
        {
            return _tileColorsData.Length;
        }
        return index;
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
        _tileColorsData = Resources.LoadAll<TileColorData>("TileColorData");
    }
}

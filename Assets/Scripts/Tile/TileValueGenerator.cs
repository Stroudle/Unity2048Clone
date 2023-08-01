using System;
using System.Collections.Generic;

public class TileValueGenerator
{
    private Dictionary<int, float> _tileValuePercentages;

    public TileValueGenerator()
    {
        _tileValuePercentages = new Dictionary<int, float>();
    }

    public void AddValueWithPercentage(int value, float percentage)
    {
        _tileValuePercentages[value] = percentage;
    }

    public int GetRandomValue()
    {
        float totalPercentage = 0f;

        foreach(var kvp in _tileValuePercentages)
        {
            totalPercentage += kvp.Value;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalPercentage);
        float cumulativePercentage = 0f;

        foreach(var kvp in _tileValuePercentages)
        {
            cumulativePercentage += kvp.Value;
            if(randomValue <= cumulativePercentage)
            {
                return kvp.Key;
            }
        }

        return 0;
    }
}
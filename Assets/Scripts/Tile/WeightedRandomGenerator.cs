using System.Collections.Generic;

public class WeightedRandomGenerator
{
    private Dictionary<int, float> _numberWeights;

    public WeightedRandomGenerator()
    {
        _numberWeights = new Dictionary<int, float>();
    }

    public void AddNumberWithWeight(int number, float percentage)
    {
        _numberWeights[number] = percentage;
    }

    public int GetRandomNumber()
    {
        float totalWeight = 0f;

        foreach(var kvp in _numberWeights)
        {
            totalWeight += kvp.Value;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach(var kvp in _numberWeights)
        {
            cumulativeWeight += kvp.Value;
            if(randomValue <= cumulativeWeight)
            {
                return kvp.Key;
            }
        }

        return 0;
    }
}
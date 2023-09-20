using System;
using System.Collections.Generic;

public class WeightedRandomGenerator
{
    private List<WeightedNumber> _weightedNumbers;

    public WeightedRandomGenerator(List<WeightedNumber> weightedNumbers)
    {
        _weightedNumbers = weightedNumbers;
    }

    public int GetRandomNumber()
    {

        float totalWeight = 0f;

        foreach(var numberWithWeight in _weightedNumbers)
        {
            totalWeight += numberWithWeight.weight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach(var numberWithWeight in _weightedNumbers)
        {
            cumulativeWeight += numberWithWeight.weight;
            if(randomValue <= cumulativeWeight)
            {
                return numberWithWeight.number;
            }
        }

        return _weightedNumbers[0].number;
    }
}

[Serializable]
public struct WeightedNumber
{
    public int number;
    public float weight;
}
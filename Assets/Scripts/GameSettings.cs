using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    public int initialTileCount;
    public List<WeightedNumber> initialTileWeights;
    public float tweenDuration;
}
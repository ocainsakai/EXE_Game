using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerDatabase", menuName = "Scriptable Objects/PokerDatabase")]
public class PokerDatabase : ScriptableObject
{
    public SerializableDictionary<PokerHandType, int > PokerHandValues;

    public int GetMultiplier(PokerHandType handType)
    {
        if (PokerHandValues.TryGetValue(handType, out int multiplier))
        {
            return multiplier;
        }
        Debug.LogError($"Poker hand type {handType} not found in database.");
        return 0; // or throw an exception
    }
}


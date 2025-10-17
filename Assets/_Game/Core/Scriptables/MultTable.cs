using CardSystem.PokerSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Mult Table", menuName = "Scriptable Objects/MultTable")]
public class MultTable : ScriptableObject
{
    [FormerlySerializedAs("Table")] public SerializableDictionary<PokerHandType, float> table;

    public float GetMult(PokerHandType type)
    {
        if( table.TryGetValue(type, out var value))
        {
            return value;
        }
        return 0f;
    }

    [ContextMenu("OnReset")]
    public void OnReset()
    {
        table = new();
        table.Add(PokerHandType.None, 0f);
        table.Add(PokerHandType.HighCard, 1f);
        table.Add(PokerHandType.OnePair, 2f);
        table.Add(PokerHandType.TwoPair, 3f);
        table.Add(PokerHandType.ThreeOfAKind, 4f);
        table.Add(PokerHandType.Straight, 5f);
        table.Add(PokerHandType.Flush, 6f);
        table.Add(PokerHandType.FullHouse, 7f);
        table.Add(PokerHandType.FourOfAKind, 8f);
        table.Add(PokerHandType.StraightFlush, 9f);
        table.Add(PokerHandType.RoyalFlush, 10f);

        table.SerializedKeys = new List<PokerHandType>(table.Keys);
        table.SerializedValues = new List<float>(table.Values);
    }


}

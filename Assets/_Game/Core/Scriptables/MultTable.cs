using CardSystem.PokerSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mult Table", menuName = "Scriptable Objects/MultTable")]
public class MultTable : ScriptableObject
{
    public SerializableDictionary<PokerHandType, float> Table;

    public float GetMult(PokerHandType type)
    {
        if( Table.TryGetValue(type, out var value))
        {
            return value;
        }
        return 0f;
    }

    [ContextMenu("OnReset")]
    public void OnReset()
    {
        Table = new();
        Table.Add(PokerHandType.None, 0f);
        Table.Add(PokerHandType.HighCard, 1f);
        Table.Add(PokerHandType.OnePair, 2f);
        Table.Add(PokerHandType.TwoPair, 3f);
        Table.Add(PokerHandType.ThreeOfAKind, 4f);
        Table.Add(PokerHandType.Straight, 5f);
        Table.Add(PokerHandType.Flush, 6f);
        Table.Add(PokerHandType.FullHouse, 7f);
        Table.Add(PokerHandType.FourOfAKind, 8f);
        Table.Add(PokerHandType.StraightFlush, 9f);
        Table.Add(PokerHandType.RoyalFlush, 10f);

        Table.SerializedKeys = new List<PokerHandType>(Table.Keys);
        Table.SerializedValues = new List<float>(Table.Values);
    }


}

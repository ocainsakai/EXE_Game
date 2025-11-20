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
        if (table.TryGetValue(type, out var value))
        {
            return value;
        }
        return 0f;
    }

    [ContextMenu("OnReset")]
    public void OnReset()
    {
        table = new();
        table.Add(PokerHandType.KhongCo, 0f);
        table.Add(PokerHandType.BaiCao, 1f);
        table.Add(PokerHandType.MotDoi, 2f);
        table.Add(PokerHandType.HaiDoi, 3f);
        table.Add(PokerHandType.SamCo, 4f);
        table.Add(PokerHandType.Sanh, 5f);
        table.Add(PokerHandType.Thung, 6f);
        table.Add(PokerHandType.CuLu, 7f);
        table.Add(PokerHandType.TuQuy, 8f);
        table.Add(PokerHandType.ThungPhaSanh, 9f);
        table.Add(PokerHandType.ThungPhaSanhHoangGia, 10f);

        table.SerializedKeys = new List<PokerHandType>(table.Keys);
        table.SerializedValues = new List<float>(table.Values);
    }
}
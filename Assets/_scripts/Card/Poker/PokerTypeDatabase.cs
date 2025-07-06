using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PokerTypeData", menuName = "Scriptable Objects/PokerTypeDatabase")]

public class PokerTypeDatabase : ScriptableObject
{
    public List<PokerTypeData> pokerTypes = new List<PokerTypeData>();
    public PokerTypeData GetData(PokerType pokerType)
    {
        return pokerTypes.Find(x => x.Type == pokerType);
    }
}

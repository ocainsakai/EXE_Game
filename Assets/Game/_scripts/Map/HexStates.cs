using Map;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexStates", menuName = "Scriptable Objects/HexStates")]
public class HexStates : ScriptableObject
{
    public List<HexState> hexStates = new List<HexState>();
    public HexState GetHexState(Vector2Int position)
    {
        return hexStates.Find(x => x.position == position);
    }
}

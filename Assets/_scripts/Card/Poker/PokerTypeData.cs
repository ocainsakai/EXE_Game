using UnityEngine;

[CreateAssetMenu(fileName = "PokerTypeData", menuName = "Scriptable Objects/PokerTypeData")]
public class PokerTypeData : ScriptableObject
{
    public PokerType Type;
    public int BaseChip;
    public int BaseMult;
}

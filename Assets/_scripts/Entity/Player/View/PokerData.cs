using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerData", menuName = "Scriptable Objects/PokerData")]

public class PokerData : ScriptableObject
{
    public ReactiveProperty<PokerHandType> PokerType = new();
    public ReactiveProperty<int> PokerMult = new();

    public void Reset()
    {
        PokerType.Value = PokerHandType.None;
        PokerMult.Value = 0;
    }
}

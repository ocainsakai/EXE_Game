using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "PokerData", menuName = "Scriptable Objects/PokerData")]

public class PokerData : ScriptableObject
{
    public PokerDatabase PokerDatabase;
    public ReactiveProperty<PokerHandType> PokerType = new();
    public ReactiveProperty<int> PokerMult = new();

    public void OnEnable()
    {
        PokerType.Subscribe(type =>
        {
            if (PokerDatabase == null)
            {
                return;
            }
            PokerMult.Value = PokerDatabase.GetMultiplier(type);
        });
    }
    public void Reset()
    {
        PokerType.Value = PokerHandType.None;
    }
}

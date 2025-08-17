using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinSO", menuName = "Scriptable Objects/CoinSO")]
public class CoinRSO : ScriptableObject
{
    public ReactiveProperty<int> onwnerCoins = new ReactiveProperty<int>(0);
}

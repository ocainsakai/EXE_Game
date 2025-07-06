using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    public ReactiveProperty<int> health = new ReactiveProperty<int>(100);
}

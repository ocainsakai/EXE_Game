using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "Scriptable Objects/GamePhase")]
public class GamePhase : ScriptableObject
{
    public ReactiveProperty<Phase> currentPhase = new ReactiveProperty<Phase>(Phase.None);
}

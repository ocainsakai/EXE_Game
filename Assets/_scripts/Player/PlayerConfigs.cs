using UnityEngine;
using UniRx;
[CreateAssetMenu(fileName = "PlayerConfigs", menuName = "Scriptable Objects/PlayerConfigs")]
public class PlayerConfigs : ScriptableObject
{
    public int MaxHealth = 100;
    public int HandSize = 8;
    public int SelectSize = 5;
}

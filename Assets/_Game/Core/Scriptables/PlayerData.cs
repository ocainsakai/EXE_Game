using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [FormerlySerializedAs("HP")] public float hp = 300;
    [Header("Currency")] 
    public int gold = 0;
}

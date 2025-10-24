using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float hp = 300;
    [Header("Currency")]
    
    public int Gold
    {
        get => PlayerSave.GetPlayerCoin();
        set =>  PlayerSave.SetPlayerCoin(value);
    }
    
}

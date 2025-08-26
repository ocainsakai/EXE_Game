using System;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public string playerName;
    public Color playerColor;
    public int playerScore;

    public PlayerData(string playerName, Color color)
    {
        this.playerName = playerName;
        this.playerColor = color;
        playerScore = 0;
    }
}

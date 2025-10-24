using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public static partial class PlayerSave
{
    private const string KEY_COIN = "PLAYERCOIN";

    public static void SetPlayerCoin(int playerCoin)
        => PlayerPrefs.SetInt(KEY_COIN, playerCoin);
    
    public static int GetPlayerCoin()
    {
        if (SecurePrefs.HasKey(KEY_COIN))
            return PlayerPrefs.GetInt(KEY_COIN, 0);
        Debug.LogError("No unlocked characters available to select.");
        return 0;
    }

}


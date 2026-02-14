using System;
using System.Collections.Generic;

namespace _Game.Core.SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        public int version = 1;
        public PlayerProgressData progress = new();
        public _Game.Core.Data.UserDeckData cardCollection = new();
        public SettingsSaveData settings = new();
    }

    [Serializable]
    public class PlayerProgressData
    {
        public int coins;
        public int level;
        // add more stats here
    }


    [Serializable]
    public class SettingsSaveData
    {
        public float masterVolume = 1f;
        public float vfxVolume = 1f;
        public float ambienceVolume = 1f;
        public string cachedPrivacyPolicy = "";
    }
}

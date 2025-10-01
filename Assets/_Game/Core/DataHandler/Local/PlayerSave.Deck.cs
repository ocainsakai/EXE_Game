using UnityEngine;

public static partial class PlayerSave
{
    // ─────────────────────────────────────────────────── Keys
    private const string KEY_SELECTED_DECK= "PLAYERSELECTEDDECK_";
    //private const string KEY_FAV_CHAR = "PLAYERFAVOURITECHARACTER_";
    //private const string KEY_MASTERY_LVL = "CHARACTERMASTERYLEVEL_";
    //private const string KEY_MASTERY_EXP = "CHARACTERMASTERYCURRENTEXP_";
    //private const string KEY_LEVEL = "CHARACTERLEVEL_";
    //private const string KEY_CUR_EXP = "CHARACTERCURRENTEXP_";
    //private const string KEY_SLOT = "ITEMSLOT_";            // +id+slot
    //private const string KEY_ITEM_LVL = "ITEMLEVEL_";           // +guid
    //private const string KEY_UPGRADE = "CHARACTERUPGRADELEVEL_";// +id+stat
    //private const string KEY_SKIN = "CHARACTERSKIN_";
    //private const string KEY_UNLOCKED_SKINS = "CHARACTERUNLOCKEDSKINS_";

    public static void SetSelectedDeck(int id) =>
        SecurePrefs.SetEncryptedInt(KEY_SELECTED_DECK, id);

    public static int GetSelectedDeck()
    {
        if (SecurePrefs.HasKey(KEY_SELECTED_DECK))
            return SecurePrefs.GetDecryptedInt(KEY_SELECTED_DECK, 0);

        // fallback: first unlocked in the DB
        foreach (var c in GameInstance.Singleton.deckData)
            if (c.CheckUnlocked) return c.DeckID;

        Debug.LogError("No unlocked characters available to select.");
        return 0;
    }
}

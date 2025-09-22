using CardSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Game/PlayerConfig")]

public class PlayerConfig : ScriptableObject
{
    public SerializableGuid Id;
    public int MaxHp;
    public int ApMax;
    public int ApRegenPerTurn;
    public int HandSize;
    public List<CardData> StartingDeck;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Game/PlayerConfig")]

public class PlayerConfig : ScriptableObject, IData
{
    public SerializableGuid Id;
    public int MaxHp;
    public int ApMax;
    public int ApRegenPerTurn;
    public int HandSize;
    public List<CardSDData> StartingDeck;

    public SerializableGuid ID => Id;
}

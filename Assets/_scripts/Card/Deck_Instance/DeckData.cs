using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Scriptable Objects/DeckData")]
public class DeckData : ScriptableObject
{
    public Sprite CardBack;
    public List<CardData> startingCards;


}

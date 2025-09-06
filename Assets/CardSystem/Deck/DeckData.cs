using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "DeckData", menuName = "Cards/Deck")]
    public class DeckData : ScriptableObject
    {
        public string DeckName;
        public List<CardData> Cards = new List<CardData>();
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "DeckData", menuName = "Cards/Deck")]
    public class DeckData : ScriptableObject
    {
        public int DeckID;
        public string DeckName;
        public Sprite DeckCover;
        public List<CardData> Cards = new List<CardData>();

        [SerializeField]
        private bool isUnlock;
        public bool CheckUnlocked
        {
            get { return isUnlock; }
        }
    }
}
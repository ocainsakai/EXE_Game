using UnityEngine;

namespace _Game.Core.Data
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Cards/CardData")]
    public class CardData : ScriptableObject
    {
        [SerializeField] private SerializableGuid id;
        public SerializableGuid Id => id;

        public string cardName;
        [TextArea] public string description;
        public Sprite artwork;
        public CardSuit suit;
        public CardRank rank;
        public CardRarity rarity;

        [ContextMenu("Generate New ID")]
        public void GenerateId()
        {
            id = SerializableGuid.NewGuid();
        }
    }
}

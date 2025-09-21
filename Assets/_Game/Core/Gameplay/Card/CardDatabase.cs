
namespace CardSystem
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CardDatabase", menuName = "Cards/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        public List<CardData> AllCards = new List<CardData>();

        public List<CardData> GetVariants(CardRank rank, CardSuit suit)
        {
            return AllCards.FindAll(c => c.Rank == rank && c.Suit == suit);
        }
#if UNITY_EDITOR
        [ContextMenu("Refresh Database")]
        public void Refresh()
        {
            AllCards.Clear();

            string[] guids = AssetDatabase.FindAssets("t:CardData"); 
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardData card = AssetDatabase.LoadAssetAtPath<CardData>(path);
                if (card != null && !AllCards.Contains(card))
                    AllCards.Add(card);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            Debug.Log($"✅ CardDatabase refreshed. Found {AllCards.Count} cards.");
        }
#endif
    }
}
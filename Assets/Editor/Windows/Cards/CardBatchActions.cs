using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace CardSystem
{
    public static class CardBatchActions
    {
        private static readonly string[] Ranks =
        {
            "Ace", "Two", "Three", "Four", "Five", "Six", "Seven",
            "Eight", "Nine", "Ten", "Jack", "Queen", "King"
        };

        private static readonly CardRank[] RankEnums =
        {
            CardRank.Ace, CardRank.Two, CardRank.Three, CardRank.Four, CardRank.Five,
            CardRank.Six, CardRank.Seven, CardRank.Eight, CardRank.Nine, CardRank.Ten,
            CardRank.Jack, CardRank.Queen, CardRank.King
        };

        private static readonly CardSuit[] Suits =
        {
            CardSuit.Hearts, CardSuit.Diamonds, CardSuit.Clubs, CardSuit.Spades
        };

        /// <summary>
        /// Generate 52 card assets (Ace–King, 4 suits)
        /// </summary>
        public static void GenerateBaseDeck(CardDatabase db, string folderPath = "Assets/CardSystem/Generated")
        {
            if (db == null)
            {
                Debug.LogError("❌ No CardDatabase assigned.");
                return;
            }

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string[] split = folderPath.Split('/');
                string parent = "Assets";
                for (int i = 1; i < split.Length; i++)
                {
                    string folder = split[i];
                    string checkPath = $"{parent}/{folder}";
                    if (!AssetDatabase.IsValidFolder(checkPath))
                        AssetDatabase.CreateFolder(parent, folder);
                    parent = checkPath;
                }
            }

            foreach (var suit in Suits)
            {
                for (int i = 0; i < Ranks.Length; i++)
                {
                    string cardName = $"{Ranks[i]} of {suit}";
                    string assetPath = $"{folderPath}/{cardName}.asset";

                    // tránh tạo trùng
                    if (File.Exists(assetPath))
                        continue;

                    var card = ScriptableObject.CreateInstance<CardData>();
                    card.Rank = RankEnums[i];
                    card.Suit = suit;
                    card.Name = cardName;
                    card.Cost = 0;

                    AssetDatabase.CreateAsset(card, assetPath);
                }
            }

            AssetDatabase.SaveAssets();
            db.Refresh();
            Debug.Log("✅ Generated 52 base cards!");
        }

        /// <summary>
        /// Auto-assign placeholder sprite for cards without art.
        /// </summary>
        public static void AutoAssignPlaceholder(CardDatabase db, Sprite placeholder)
        {
            if (db == null || placeholder == null) return;
            AutoAssignPlaceholder(db.AllCards, placeholder);
        }
        public static void AutoAssignPlaceholders(CardDatabase db, List<Sprite> sprites)
        {
            if (db == null || sprites == null || sprites.Count == 0) return;

            int count = Mathf.Min(db.AllCards.Count, sprites.Count);

            for (int i = 0; i < count; i++)
            {
                var card = db.AllCards[i];
                var sprite = sprites[i];
                if (card != null && sprite != null)
                {
                    card.Art = sprite;
                    EditorUtility.SetDirty(card);
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Assigned {count} sprites to {db.AllCards.Count} cards.");
        }

        // Dùng cho list card bất kỳ
        public static void AutoAssignPlaceholder(List<CardData> cards, Sprite placeholder)
        {
            if (cards == null || placeholder == null) return;

            foreach (var card in cards)
            {
                if (card != null && card.Art == null)
                {
                    card.Art = placeholder;
                    EditorUtility.SetDirty(card);
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Placeholder assigned to {cards.Count} cards.");
        }
        /// <summary>
        /// Validate database: empty names, duplicates, etc.
        /// </summary>
        public static void ValidateDatabase(CardDatabase db)
        {
            if (db == null) return;

            HashSet<string> seen = new HashSet<string>();
            foreach (var card in db.AllCards)
            {
                if (string.IsNullOrEmpty(card.Name))
                    Debug.LogWarning($"⚠️ Card {card} has empty name!");

                if (!seen.Add(card.Name))
                    Debug.LogWarning($"⚠️ Duplicate card name: {card.Name}");
            }

            Debug.Log("✅ Validation complete.");
        }
    }
}

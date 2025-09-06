using UnityEditor;
using UnityEngine;
using System.Linq;

namespace CardSystem
{
    public class CardOverviewTab : ICardEditorTab
    {
        public string TabName => "Overview";
        private CardDatabase database;
        public void DrawGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Card Database", EditorStyles.boldLabel);
            database = (CardDatabase)EditorGUILayout.ObjectField("...", database, typeof(CardDatabase), false);
            EditorGUILayout.EndVertical();
            GUILayout.Label("Card Overview", EditorStyles.boldLabel);

            if (database == null)
            {
                GUILayout.Label("⚠ No database assigned.");
                return;
            }

            var all = database.AllCards;
            if (all == null || all.Count == 0)
            {
                GUILayout.Label("Database empty.");
                return;
            }

            var bySuit = all.GroupBy(c => c.Suit).ToDictionary(g => g.Key, g => g.Count());
            var byRank = all.GroupBy(c => c.Rank).ToDictionary(g => g.Key, g => g.Count());

            GUILayout.Label("By Suit:");
            foreach (var kv in bySuit)
                GUILayout.Label($"{kv.Key}: {kv.Value}");

            GUILayout.Space(10);

            GUILayout.Label("By Rank:");
            foreach (var kv in byRank)
                GUILayout.Label($"{kv.Key}: {kv.Value}");
        }
    }
}
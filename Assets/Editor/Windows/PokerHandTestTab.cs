using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CardSystem.PokerSystem;
using System.Linq;

namespace CardSystem
{
    public class PokerHandTestTab : ICardEditorTab
    {
        public string TabName => "Poker Test";

        private List<CardData> testHand = new List<CardData>();

        public void DrawGUI()
        {
            GUILayout.Label("Poker Hand Tester", EditorStyles.boldLabel);

            //if (database == null)
            //{
            //    GUILayout.Label("⚠ No database assigned.");
            //    return;
            //}

            GUILayout.Label("Drag cards here:");
            for (int i = 0; i < testHand.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                testHand[i] = (CardData)EditorGUILayout.ObjectField(testHand[i], typeof(CardData), false);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                    testHand.RemoveAt(i);
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Card Slot"))
                testHand.Add(null);

            if (GUILayout.Button("Evaluate Hand"))
                EvaluateHand(testHand);
        }

        private void EvaluateHand(List<CardData> hand)
        {
            if (hand.Count == 0 || hand.TrueForAll(c => c == null))
            {
                Debug.LogWarning("⚠ No cards selected!");
                return;
            }

            // TODO: Replace with real PokerEvaluator

            var result = PokerEvaluator.Evaluate(hand.Select(x => x.Mask));
            Debug.Log("🃏 Hand contains " + hand.Count + " cards.");
            Debug.Log("🃏 Hand result " + result.HandType + " type.");
        }
    }
}
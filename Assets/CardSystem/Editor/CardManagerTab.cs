using System.IO;
using UnityEditor;
using UnityEngine;

namespace CardSystem
{
    public class CardManagerTab : ICardEditorTab
    {
        public string TabName => "Manager";

        private CardDatabase database;
        private bool initialized = false;

        private const string PrefKey_DBGuid = "CardSystem.DatabaseGUID";
        private const string DefaultCardsFolder = "Assets/Cards";

        // Dùng wrapper state
        private CardListViewState viewState = new CardListViewState();

        public void DrawGUI()
        {
            if (!initialized)
            {
                initialized = true;
                LoadDatabaseFromPrefs();
            }

            CardEditorUI.DrawDatabaseHeader(database);

            CardEditorUI.DrawFilterUI(
                ref viewState.searchTerm,
                ref viewState.useFilter,
                ref viewState.filterRank,
                ref viewState.filterSuit
            );

            if (database == null)
            {
                GUILayout.Label("⚠ No database assigned.");
                return;
            }

            CardEditorUI.DrawCardList(database.AllCards, viewState);
        }

        private void LoadDatabaseFromPrefs()
        {
            var guid = EditorPrefs.GetString(PrefKey_DBGuid, string.Empty);
            if (string.IsNullOrEmpty(guid)) return;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return;

            database = AssetDatabase.LoadAssetAtPath<CardDatabase>(path);
        }

    }
}

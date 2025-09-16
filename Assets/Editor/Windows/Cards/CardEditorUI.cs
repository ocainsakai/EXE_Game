using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CardSystem
{
    public enum CardViewMode { OneLine, Grid, FullDetail }
    public enum SortBy { None, Rank, Suit, Name, Cost }

    public static class CardEditorUI
    {
        private const string PrefKey_DBGuid = "CardSystem.DatabaseGUID";

        // =======================
        // Draw Database Header
        // =======================
        public static void DrawDatabaseHeader(CardDatabase database)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Card Database", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            var newDb = (CardDatabase)EditorGUILayout.ObjectField("Current Database", database, typeof(CardDatabase), false);
            if (EditorGUI.EndChangeCheck())
            {
                SetDatabase(newDb, ref database);
            }

            if (database == null)
            {
                if (GUILayout.Button("Create New Database", GUILayout.Height(22)))
                    CreateNewDatabaseInteractively(database);
            }
            else
            {
                if (GUILayout.Button("Refresh", GUILayout.Width(80)))
                    database.Refresh();

                if (GUILayout.Button(EditorGUIUtility.IconContent("SceneAsset Icon"), GUILayout.Width(24), GUILayout.Height(20)))
                    Selection.activeObject = database;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        public static void CreateNewDatabaseInteractively(CardDatabase database)
        {
            var defaultPath = "Assets/CardDatabase.asset";
            var path = EditorUtility.SaveFilePanelInProject(
                "Create Card Database",
                "CardDatabase",
                "asset",
                "Chọn nơi lưu CardDatabase",
                Path.GetDirectoryName(defaultPath)
            );

            if (!string.IsNullOrEmpty(path))
            {
                var db = ScriptableObject.CreateInstance<CardDatabase>();
                AssetDatabase.CreateAsset(db, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                SetDatabase(db, ref database);
                EditorGUIUtility.PingObject(db);
            }
        }

        public static void SetDatabase(CardDatabase db, ref CardDatabase database)
        {
            database = db;
            if (database != null)
            {
                var path = AssetDatabase.GetAssetPath(database);
                var guid = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid))
                    EditorPrefs.SetString(PrefKey_DBGuid, guid);
            }
            else
            {
                EditorPrefs.DeleteKey(PrefKey_DBGuid);
            }
        }
        // ========== Info UI ==========
        public static void DrawCardMainInfo(CardData card, bool id = true)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"{card.name}", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            DrawSelectButton(card);
            EditorGUILayout.EndHorizontal();
            if (id)
            {
                EditorGUILayout.LabelField($"ID: {card.CardID.ToHexString()}");
            }
            EditorGUILayout.EndVertical();
        }

        public static void DrawSelectButton(Object card)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(EditorGUIUtility.IconContent("SceneAsset Icon"), GUILayout.Width(24), GUILayout.Height(20)))
            {
                Selection.activeObject = card;
                EditorGUIUtility.PingObject(card);
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_ViewToolZoom"), GUILayout.Width(24), GUILayout.Height(20)))
            {
                EditorGUIUtility.PingObject(card);
            }
            EditorGUILayout.EndHorizontal();
        }
        // =======================
        // Toolbar
        // =======================
        public static void DrawCardListToolbar(CardListViewState state, List<CardData> filtered)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            // View mode toggle
            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.OneLine, "One-line", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.OneLine;
            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.Grid, "Grid", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.Grid;
            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.FullDetail, "Full", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.FullDetail;

            GUILayout.FlexibleSpace();

            // Sort
            state.sortBy = (SortBy)EditorGUILayout.EnumPopup(state.sortBy, GUILayout.Width(110));
            state.sortAscending = GUILayout.Toggle(state.sortAscending, state.sortAscending ? "Asc" : "Desc", EditorStyles.toolbarButton, GUILayout.Width(50));

            // Thumb / art
            if (state.cardViewMode != CardViewMode.FullDetail)
            {
                GUILayout.Space(6);
                GUILayout.Label("Thumb");
                state.thumbSize = EditorGUILayout.IntSlider(state.thumbSize, 32, 256, GUILayout.Width(180));
                state.showArt = GUILayout.Toggle(state.showArt, "Art", EditorStyles.toolbarButton, GUILayout.Width(50));
            }

            // Quick select buttons
            if (GUILayout.Button(EditorGUIUtility.IconContent("SceneAsset Icon"), GUILayout.Width(24), GUILayout.Height(20)))
                Selection.objects = filtered.Cast<Object>().ToArray();

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_ViewToolZoom"), GUILayout.Width(24), GUILayout.Height(20)))
                foreach (var s in Selection.objects) EditorGUIUtility.PingObject(s);

            EditorGUILayout.EndHorizontal();
        }

        // ========== Filter ==========
        public static void DrawFilterUI(
            ref string searchTerm,
            ref bool useFilter,
            ref CardRankFilter filterRank,
            ref CardSuitFilter filterSuit)
        {
            EditorGUILayout.BeginVertical("box");

            // Row 1: Search + Use Filter
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search", GUILayout.Width(50));
            searchTerm = GUILayout.TextField(searchTerm);
            useFilter = EditorGUILayout.ToggleLeft("Use Filter", useFilter, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            // Row 2: Rank + Suit (chỉ hiện khi có filter)
            if (useFilter)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rank", GUILayout.Width(40));
                filterRank = (CardRankFilter)EditorGUILayout.EnumPopup(filterRank, GUILayout.Width(100));
                GUILayout.Label("Suit", GUILayout.Width(40));
                filterSuit = (CardSuitFilter)EditorGUILayout.EnumPopup(filterSuit, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }
        // =======================
        // Sorting
        // =======================
        public static IEnumerable<CardData> ApplyFilter(
            IEnumerable<CardData> allCards,
            string searchTerm,
            bool useFilter,
            CardRankFilter filterRank,
            CardSuitFilter filterSuit)
        {
            var result = allCards;

            if (!string.IsNullOrEmpty(searchTerm))
                result = result.Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));

            if (useFilter)
            {
                if (filterRank != CardRankFilter.All)
                    result = result.Where(c => c.Rank == (CardRank)filterRank);

                if (filterSuit != CardSuitFilter.All)
                    result = result.Where(c => c.Suit == (CardSuit)filterSuit);
            }

            return result;
        }

        // ========== Card List Viewer ==========
        public static void DrawCardList(IEnumerable<CardData> allCards, CardListViewState state)
        {
            var filtered = ApplyFilter(allCards, state.searchTerm, state.useFilter, state.filterRank, state.filterSuit).ToList();

            DrawToolbar(state, filtered);

            // Sorting
            filtered = ApplySorting(filtered, state);

            EditorGUILayout.LabelField($"Found {filtered.Count} cards", EditorStyles.miniBoldLabel);

            state.scrollPos = GUILayout.BeginScrollView(state.scrollPos, GUILayout.MinHeight(256), GUILayout.MaxHeight(1000));

            switch (state.cardViewMode)
            {
                case CardViewMode.OneLine:
                    DrawOneLineList(filtered, state);
                    break;
                case CardViewMode.Grid:
                    DrawGridList(filtered, state);
                    break;
                case CardViewMode.FullDetail:
                    DrawFullDetailList(filtered, state);
                    break;
            }

            GUILayout.EndScrollView();
        }

        // =======================
        // Toolbar + Sort
        // =======================
        private static void DrawToolbar(CardListViewState state, List<CardData> filtered)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.OneLine, "One-line", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.OneLine;
            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.Grid, "Grid", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.Grid;
            if (GUILayout.Toggle(state.cardViewMode == CardViewMode.FullDetail, "Full", EditorStyles.toolbarButton))
                state.cardViewMode = CardViewMode.FullDetail;

            GUILayout.FlexibleSpace();

            state.sortBy = (SortBy)EditorGUILayout.EnumPopup(state.sortBy, GUILayout.Width(110));
            state.sortAscending = GUILayout.Toggle(state.sortAscending, state.sortAscending ? "Asc" : "Desc", EditorStyles.toolbarButton, GUILayout.Width(50));

            if (state.cardViewMode != CardViewMode.FullDetail)
            {
                GUILayout.Space(6);
                GUILayout.Label("Thumb");
                state.thumbSize = EditorGUILayout.IntSlider(state.thumbSize, 32, 256, GUILayout.Width(180));
                state.showArt = GUILayout.Toggle(state.showArt, "Art", EditorStyles.toolbarButton, GUILayout.Width(50));
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("SceneAsset Icon"), GUILayout.Width(24), GUILayout.Height(20)))
                Selection.objects = filtered.Cast<Object>().ToArray();

            if (GUILayout.Button(EditorGUIUtility.IconContent("d_ViewToolZoom"), GUILayout.Width(24), GUILayout.Height(20)))
                foreach (var s in Selection.objects) EditorGUIUtility.PingObject(s);

            EditorGUILayout.EndHorizontal();
        }
        // =======================
        // Sorting
        // =======================
        private static List<CardData> ApplySorting(List<CardData> cards, CardListViewState state)
        {
            if (state.sortBy == SortBy.None) return cards;

            return state.sortBy switch
            {
                SortBy.Rank => (state.sortAscending ? cards.OrderBy(c => c.Rank) : cards.OrderByDescending(c => c.Rank)).ToList(),
                SortBy.Suit => (state.sortAscending ? cards.OrderBy(c => c.Suit) : cards.OrderByDescending(c => c.Suit)).ToList(),
                SortBy.Name => (state.sortAscending ? cards.OrderBy(c => c.Name) : cards.OrderByDescending(c => c.Name)).ToList(),
                SortBy.Cost => (state.sortAscending ? cards.OrderBy(c => c.Cost) : cards.OrderByDescending(c => c.Cost)).ToList(),
                _ => cards
            };
        }

        // =======================
        // One-line Mode
        // =======================
        private static void DrawOneLineList(IEnumerable<CardData> cards, CardListViewState state)
        {
            foreach (var card in cards)
            {
                GUILayout.BeginHorizontal("box", GUILayout.Height(Mathf.Max(state.thumbSize, 40)));
                if (state.showArt)
                    GUILayout.Label(card.Art ? card.Art.texture : Texture2D.grayTexture, GUILayout.Width(state.thumbSize), GUILayout.Height(state.thumbSize));

                DrawCardMainInfo(card);
                GUILayout.EndHorizontal();
            }
        }

        // =======================
        // Grid Mode
        // =======================
        private static void DrawGridList(IList<CardData> cards, CardListViewState state)
        {
            float areaWidth = EditorGUIUtility.currentViewWidth - 174;
            int cols = Mathf.Max(1, Mathf.FloorToInt(areaWidth / (state.thumbSize + 16)));
            int i = 0;

            EditorGUILayout.BeginVertical();
            while (i < cards.Count)
            {
                EditorGUILayout.BeginHorizontal();
                for (int c = 0; c < cols && i < cards.Count; c++, i++)
                {
                    var card = cards[i];
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(state.thumbSize + 12));
                    if (state.showArt)
                        GUILayout.Label(card.Art ? card.Art.texture : Texture2D.grayTexture, GUILayout.Width(state.thumbSize), GUILayout.Height(state.thumbSize));

                    GUILayout.Label($"{card.Rank}\n{card.Suit}", EditorStyles.miniLabel, GUILayout.Height(30), GUILayout.Width(state.thumbSize));
                    GUILayout.Label(card.Name, EditorStyles.wordWrappedLabel, GUILayout.Width(state.thumbSize));
                    DrawSelectButton(card);
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        // =======================
        // FullDetail Mode
        // =======================
        private static void DrawFullDetailList(IEnumerable<CardData> cards, CardListViewState state)
        {
            foreach (var card in cards)
            {
                GUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                if (card.Art)
                    GUILayout.Label(card.Art.texture, GUILayout.Width(150), GUILayout.Height(150));

                EditorGUILayout.BeginVertical();
                DrawCardMainInfo(card);
                EditorGUILayout.LabelField($"Name: {card.Name}");
                EditorGUILayout.LabelField($"Cost: {card.Cost}");
                EditorGUILayout.LabelField("Description:");
                EditorGUILayout.HelpBox(string.IsNullOrEmpty(card.Description) ? "(no description)" : card.Description, MessageType.None);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                DrawSelectButton(card);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                GUILayout.Space(6);
            }
        }

    }
}


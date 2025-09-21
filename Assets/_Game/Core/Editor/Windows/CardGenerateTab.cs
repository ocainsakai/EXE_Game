using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CardSystem
{
    public class CardGeneratorTab : ICardEditorTab
    {
        public string TabName => "Generate";
        private const string PrefKey_DBGuid = "CardSystem.DatabaseGUID";

        private string folderPath = DefaultCardsFolder;
        private const string DefaultCardsFolder = "Assets/Cards";

        // Cho auto-assign sprite
        private Sprite placeholderSprite;
        private List<Sprite> spriteList = new List<Sprite>();
        private List<CardData> cardList = new List<CardData>();
        private Vector2 spriteScroll;

        private CardDatabase database;

        public void DrawGUI()
        {
            GUILayout.Label("Card Generation & Tools", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            LoadDatabaseFromPrefs();
            // --- Select Database ---
            EditorGUILayout.BeginHorizontal();
            CardEditorUI.DrawDatabaseHeader(database);
            // --- Validate Section ---
            if (database != null && GUILayout.Button("Validate Database", GUILayout.Width(150)))
            {
                CardBatchActions.ValidateDatabase(database);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // --- Generate Section ---
            DrawGenerateDeckSection();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // --- Auto Assign Section ---
            DrawAutoAssignSection();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            
        }
        private void LoadDatabaseFromPrefs()
        {
            var guid = EditorPrefs.GetString(PrefKey_DBGuid, string.Empty);
            if (string.IsNullOrEmpty(guid)) return;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return;

            database = AssetDatabase.LoadAssetAtPath<CardDatabase>(path);
        }
        private void DrawGenerateDeckSection()
        {
            GUILayout.Label("Generate 52 Playing Cards", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            folderPath = ToRelativePath(folderPath);
            folderPath = EditorGUILayout.TextField("Save Folder", folderPath);

            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                string selected = EditorUtility.OpenFolderPanel("Select Card Save Folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(selected))
                {
                    folderPath = ToRelativePath(selected);
                }
            }

            if (GUILayout.Button("Generate Full Deck", GUILayout.Width(150)) )
            {
                GenerateDeck();
            }
            EditorGUILayout.EndHorizontal();

        }
        public enum SpriteAssignMode
        {
            Placeholder,   // 1 sprite cho tất cả card chưa có Art
            IndexOrder,    // List sprite theo thứ tự index
            NameMatching,  // Ghép sprite.name == card.Name
            RankSuitOrder  // Ghép sprite theo thứ tự Rank + Suit
        }
        private SpriteAssignMode assignMode = SpriteAssignMode.Placeholder;
        private void DrawAutoAssignSection()
        {
            GUILayout.Label("Auto Assign Sprites", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            // popup chọn mode
            assignMode = (SpriteAssignMode)EditorGUILayout.EnumPopup("Assign Mode", assignMode, GUILayout.MinWidth(150));
            if (GUILayout.Button("Assign", GUILayout.Width(100)))
            {
                DoAssign();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // phần UI cho placeholder hoặc sprite list
            switch (assignMode)
            {
                case SpriteAssignMode.Placeholder:
                    placeholderSprite = (Sprite)EditorGUILayout.ObjectField("Placeholder", placeholderSprite, typeof(Sprite), false);
                    break;

                case SpriteAssignMode.IndexOrder:
                case SpriteAssignMode.NameMatching:
                case SpriteAssignMode.RankSuitOrder:
                    cardList = database.AllCards;
                    cardList.SortBySuit();
                    DrawSpriteListEditor();
                    break;
            }
        }

        private void DrawSpriteListEditor()
        {
            EditorGUILayout.LabelField("Sprite List", EditorStyles.miniBoldLabel);

            // Khu vực drag & drop
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag & Drop Sprites or Folder Here", EditorStyles.helpBox);
            HandleDragAndDropSprites(dropArea);
            // Hiển thị danh sách sprite
            spriteScroll = EditorGUILayout.BeginScrollView(spriteScroll, GUILayout.Height(150));
            cardList = database.AllCards;
            cardList.SortBySuit();
            for (int i = 0; i < cardList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                cardList[i] = (CardData)EditorGUILayout.ObjectField($"Card {i + 1}", cardList[i], typeof(CardData), false);

                if (i < spriteList.Count)
                {


                    spriteList[i] = (Sprite)EditorGUILayout.ObjectField($"Sprite {i + 1}", spriteList[i], typeof(Sprite), false);

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        spriteList.RemoveAt(i);
                        i--;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            //EditorGUILayout.EndHorizontal();
            // Nút xóa hết
            if (spriteList.Count > 0 && GUILayout.Button("Clear All"))
            {
                spriteList.Clear();
            }
        }

        private void HandleDragAndDropSprites(Rect dropArea)
        {
            Event evt = Event.current;
            if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
            {
                if (dropArea.Contains(evt.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (string path in DragAndDrop.paths)
                        {
                            if (AssetDatabase.IsValidFolder(path))
                            {
                                // Nếu là folder → lấy tất cả sprite trong đó
                                string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { path });
                                foreach (string guid in guids)
                                {
                                    string spPath = AssetDatabase.GUIDToAssetPath(guid);
                                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spPath);
                                    if (sprite != null && !spriteList.Contains(sprite))
                                        spriteList.Add(sprite);
                                }
                            }
                            else
                            {
                                // Nếu là file asset → thử load Sprite
                                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                                if (sprite != null && !spriteList.Contains(sprite))
                                    spriteList.Add(sprite);
                            }
                        }
                        evt.Use();
                    }
                }
            }
        }

        private void DoAssign()
        {
            if (database == null)
            {
                Debug.LogWarning("⚠️ Please select Database first.");
                return;
            }

            switch (assignMode)
            {
                case SpriteAssignMode.Placeholder:
                    if (placeholderSprite != null)
                        CardBatchActions.AutoAssignPlaceholder(database, placeholderSprite);
                    else
                        Debug.LogWarning("⚠️ Please select Placeholder Sprite.");
                    break;

                case SpriteAssignMode.IndexOrder:
                    AutoAssignByIndex(database, spriteList);
                    break;

                case SpriteAssignMode.NameMatching:
                    AutoAssignByName(database, spriteList);
                    break;

                case SpriteAssignMode.RankSuitOrder:
                    AutoAssignByRankSuit(database, spriteList);
                    break;
            }
        }
        private void AutoAssignByIndex(CardDatabase db, List<Sprite> sprites)
        {
            int count = Mathf.Min(db.AllCards.Count, sprites.Count);

            for (int i = 0; i < count; i++)
            {
                db.AllCards[i].Art = sprites[i];
                EditorUtility.SetDirty(db.AllCards[i]);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Assigned {count} sprites by index order.");
        }

        private void AutoAssignByName(CardDatabase db, List<Sprite> sprites)
        {
            var map = new Dictionary<string, Sprite>();
            foreach (var sprite in sprites)
            {
                if (sprite != null && !map.ContainsKey(sprite.name))
                    map[sprite.name] = sprite;
            }

            int assigned = 0;
            foreach (var card in db.AllCards)
            {
                if (map.TryGetValue(card.Name, out var sprite))
                {
                    card.Art = sprite;
                    EditorUtility.SetDirty(card);
                    assigned++;
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Assigned {assigned} sprites by name matching.");
        }

        private void AutoAssignByRankSuit(CardDatabase db, List<Sprite> sprites)
        {
            int count = Mathf.Min(db.AllCards.Count, sprites.Count);
            //var cards = db.AllCards.OrderBy();
            // đảm bảo AllCards được sort trước
            for (int i = 0; i < count; i++)
            {
                db.AllCards[i].Art = sprites[i];
                EditorUtility.SetDirty(db.AllCards[i]);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Assigned {count} sprites by Rank+Suit order.");
        }

       
        private string ToRelativePath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return "Assets";

            fullPath = fullPath.Replace("\\", "/"); // fix cho Windows
            if (fullPath.StartsWith(Application.dataPath))
            {
                return "Assets" + fullPath.Substring(Application.dataPath.Length);
            }
            return fullPath; // fallback
        }

        private void GenerateDeck()
        {
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.Refresh();
            }

            foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in System.Enum.GetValues(typeof(CardRank)))
                {
                    var card = ScriptableObject.CreateInstance<CardData>();
                    card.Suit = suit;
                    card.Rank = rank;
                    card.Name = $"{rank} of {suit}";
                    card.Description = $"This is the {rank} of {suit}.";

                    string assetPath = Path.Combine(folderPath, $"{rank}_of_{suit}.asset");
                    AssetDatabase.CreateAsset(card, assetPath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Generated 52 cards in " + folderPath);

            if (database != null)
                database.Refresh();
        }
    }
}

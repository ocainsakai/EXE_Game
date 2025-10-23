using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using _Game.Addons.Deck.Scripts;

public class CardSpriteUpdater : EditorWindow
{
    private DefaultAsset cardDataFolder;
    private DefaultAsset newSpriteFolder;
    private CardNameMapper nameMapper;
    
    private bool useNameMapper = true;
    private bool useFuzzyMatch = true;
    private bool scanSubfolders = true; // NEW: Quét cả subfolder
    private bool showDebugInfo = false;
    private string manualPrefix = "";
    private string manualSuffix = "";
    
    private Vector2 scrollPosition;
    private List<UpdateItem> updateQueue = new List<UpdateItem>();
    private bool showPreview = false;
    
    private class UpdateItem
    {
        public ScriptableObject cardData;
        public Sprite oldSprite;
        public Sprite newSprite;
        public string cardName;
        public string matchedSpriteName;
        public bool willUpdate;
        public string status;
        public List<string> triedNames = new List<string>();
        public string spritePath; // NEW: Đường dẫn sprite để debug
    }

    [MenuItem("Tools/Card Sprite Updater")]
    public static void ShowWindow()
    {
        var window = GetWindow<CardSpriteUpdater>("Card Sprite Updater");
        window.minSize = new Vector2(600, 700);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        // Header
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.fontSize = 16;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("🎴 Card Sprite Batch Updater", headerStyle);
        
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "Tool này tự động thay thế sprites trong Card SOs với hệ thống mapping thông minh\n" +
            "(Ace of Diamonds → Xì Chuồn, 2 Bích, etc.)",
            MessageType.Info
        );
        
        EditorGUILayout.Space(10);

        // Input Section
        DrawInputSection();
        
        EditorGUILayout.Space(10);

        // Matching Options
        DrawMatchingOptions();
        
        EditorGUILayout.Space(10);

        // Buttons
        DrawButtons();
        
        EditorGUILayout.Space(10);

        // Preview Section
        if (showPreview && updateQueue.Count > 0)
        {
            DrawPreviewSection();
        }
    }

    private void DrawInputSection()
    {
        EditorGUILayout.LabelField("📁 Folders & Config", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        
        cardDataFolder = EditorGUILayout.ObjectField(
            new GUIContent("Card Data Folder", "Thư mục chứa các Card ScriptableObject"),
            cardDataFolder,
            typeof(DefaultAsset),
            false
        ) as DefaultAsset;
        
        newSpriteFolder = EditorGUILayout.ObjectField(
            new GUIContent("New Sprite Folder", "Thư mục chứa sprites mới"),
            newSpriteFolder,
            typeof(DefaultAsset),
            false
        ) as DefaultAsset;

        nameMapper = EditorGUILayout.ObjectField(
            new GUIContent("Name Mapper", "CardNameMapper SO để convert tên (Ace→Xì, Diamonds→Chuồn)"),
            nameMapper,
            typeof(CardNameMapper),
            false
        ) as CardNameMapper;
        
        if (EditorGUI.EndChangeCheck())
        {
            updateQueue.Clear();
            showPreview = false;
        }

        // Quick create mapper button
        if (nameMapper == null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("⚠️ Chưa có Name Mapper!", MessageType.Warning);
            if (GUILayout.Button("Create New", GUILayout.Width(100), GUILayout.Height(40)))
            {
                CreateNewMapper();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawMatchingOptions()
    {
        EditorGUILayout.LabelField("⚙️ Matching Options", EditorStyles.boldLabel);
        
        EditorGUI.BeginDisabledGroup(nameMapper == null);
        useNameMapper = EditorGUILayout.Toggle(
            new GUIContent("Use Name Mapper", "Dùng CardNameMapper để convert tên tự động"),
            useNameMapper
        );
        EditorGUI.EndDisabledGroup();

        if (useNameMapper && nameMapper != null)
        {
            EditorGUI.indentLevel++;
            useFuzzyMatch = EditorGUILayout.Toggle(
                new GUIContent("Fuzzy Match", "Thử nhiều variations (2_Bích, 2_b, 2bích...)"),
                useFuzzyMatch
            );
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox(
                $"Pattern: {nameMapper.spriteNamePattern}\n" +
                $"Example: 'Ace of Diamonds' → '{nameMapper.ConvertToSpriteName("Ace of Diamonds")}'",
                MessageType.None
            );

            // Test button
            if (GUILayout.Button("🧪 Test Mapper", GUILayout.Height(25)))
            {
                TestMapper();
            }
        }
        else
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Manual Pattern:", EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            manualPrefix = EditorGUILayout.TextField("Prefix", manualPrefix, GUILayout.Width(150));
            EditorGUILayout.LabelField("[CardName]", GUILayout.Width(80));
            manualSuffix = EditorGUILayout.TextField("Suffix", manualSuffix, GUILayout.Width(150));
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.Space(5);
        
        // NEW: Subfolder scanning option
        scanSubfolders = EditorGUILayout.Toggle(
            new GUIContent("Scan Subfolders", "Quét tất cả subfolder trong Sprite Folder"),
            scanSubfolders
        );
        
        showDebugInfo = EditorGUILayout.Toggle(
            new GUIContent("Show Debug Info", "Hiển thị thông tin debug chi tiết"),
            showDebugInfo
        );
    }

    private void DrawButtons()
    {
        bool canScan = cardDataFolder != null && newSpriteFolder != null;
        
        EditorGUI.BeginDisabledGroup(!canScan);
        
        GUI.backgroundColor = new Color(0.4f, 0.7f, 1f);
        if (GUILayout.Button("🔍 Scan and Preview", GUILayout.Height(35)))
        {
            ScanForUpdates();
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUI.EndDisabledGroup();
        
        if (!canScan)
        {
            EditorGUILayout.HelpBox("⚠️ Vui lòng chọn Card Data Folder và New Sprite Folder!", MessageType.Warning);
        }
        
        EditorGUILayout.Space(5);
        
        EditorGUI.BeginDisabledGroup(updateQueue.Count == 0 || !updateQueue.Any(u => u.willUpdate));
        
        GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);
        if (GUILayout.Button("✅ Apply Updates", GUILayout.Height(35)))
        {
            if (EditorUtility.DisplayDialog(
                "Confirm Update",
                $"Bạn sắp cập nhật {updateQueue.Count(u => u.willUpdate)} lá bài.\n\nĐiều này không thể hoàn tác. Tiếp tục?",
                "Yes, Update",
                "Cancel"))
            {
                ApplyUpdates();
            }
        }
        GUI.backgroundColor = Color.white;
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space(5);
        
        // Export report button
        if (updateQueue.Count > 0)
        {
            if (GUILayout.Button("📄 Export Report", GUILayout.Height(25)))
            {
                ExportReport();
            }
        }
    }

    private void DrawPreviewSection()
    {
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"📋 Preview ({updateQueue.Count} items)", EditorStyles.boldLabel);
        
        // Statistics
        int willUpdate = updateQueue.Count(u => u.willUpdate);
        int notFound = updateQueue.Count(u => !u.willUpdate && u.newSprite == null);
        int noChange = updateQueue.Count(u => !u.willUpdate && u.newSprite != null);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox($"✅ Will Update: {willUpdate}", MessageType.None);
        EditorGUILayout.HelpBox($"⚠️ Not Found: {notFound}", MessageType.None);
        EditorGUILayout.HelpBox($"ℹ️ No Change: {noChange}", MessageType.None);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Filter options
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Show All", EditorStyles.miniButtonLeft))
        {
            // Already showing all
        }
        if (GUILayout.Button("Only Updates", EditorStyles.miniButtonMid))
        {
            // Filter logic can be added here
        }
        if (GUILayout.Button("Only Not Found", EditorStyles.miniButtonRight))
        {
            // Filter logic can be added here
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Scrollable list
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        
        foreach (var item in updateQueue)
        {
            DrawUpdateItem(item);
        }
        
        EditorGUILayout.EndScrollView();
    }

    private void DrawUpdateItem(UpdateItem item)
    {
        Color bgColor = item.willUpdate ? new Color(0.8f, 1f, 0.8f) : 
                        item.newSprite == null ? new Color(1f, 0.9f, 0.8f) : Color.white;
        
        GUI.backgroundColor = bgColor;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUI.backgroundColor = Color.white;
        
        EditorGUILayout.BeginHorizontal();
        
        // Card name
        EditorGUILayout.BeginVertical(GUILayout.Width(150));
        EditorGUILayout.LabelField(item.cardName, EditorStyles.boldLabel);
        if (!string.IsNullOrEmpty(item.matchedSpriteName))
        {
            GUIStyle miniStyle = new GUIStyle(EditorStyles.miniLabel);
            miniStyle.normal.textColor = Color.gray;
            EditorGUILayout.LabelField($"→ {item.matchedSpriteName}", miniStyle);
        }
        EditorGUILayout.EndVertical();
        
        // Old sprite preview
        if (item.oldSprite != null)
        {
            Rect rect = GUILayoutUtility.GetRect(50, 50);
            EditorGUI.DrawPreviewTexture(rect, item.oldSprite.texture);
        }
        else
        {
            EditorGUILayout.LabelField("(No Sprite)", GUILayout.Width(50));
        }
        
        EditorGUILayout.LabelField("→", GUILayout.Width(20));
        
        // New sprite preview
        if (item.newSprite != null)
        {
            Rect rect = GUILayoutUtility.GetRect(50, 50);
            EditorGUI.DrawPreviewTexture(rect, item.newSprite.texture);
            EditorGUILayout.LabelField(item.newSprite.name, GUILayout.Width(120));
        }
        else
        {
            EditorGUILayout.LabelField("❌ Not Found", GUILayout.Width(120));
        }
        
        // Status
        GUIStyle statusStyle = new GUIStyle(EditorStyles.miniLabel);
        statusStyle.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField(item.status, statusStyle);
        
        EditorGUILayout.EndHorizontal();

        // Show tried names if not found or debug mode
        if ((item.newSprite == null || showDebugInfo) && item.triedNames.Count > 0)
        {
            GUIStyle triedStyle = new GUIStyle(EditorStyles.miniLabel);
            triedStyle.normal.textColor = new Color(0.7f, 0.4f, 0.4f);
            
            int displayCount = showDebugInfo ? item.triedNames.Count : Mathf.Min(3, item.triedNames.Count);
            string tried = string.Join(", ", item.triedNames.Take(displayCount));
            if (item.triedNames.Count > displayCount)
            {
                tried += $" ... (+{item.triedNames.Count - displayCount} more)";
            }
            
            EditorGUILayout.LabelField($"  Tried: {tried}", triedStyle);
        }
        
        // Show sprite path in debug mode
        if (showDebugInfo && !string.IsNullOrEmpty(item.spritePath))
        {
            GUIStyle pathStyle = new GUIStyle(EditorStyles.miniLabel);
            pathStyle.normal.textColor = new Color(0.4f, 0.4f, 0.7f);
            EditorGUILayout.LabelField($"  Path: {item.spritePath}", pathStyle);
        }
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(2);
    }

    private void ScanForUpdates()
    {
        updateQueue.Clear();
        
        EditorUtility.DisplayProgressBar("Scanning Sprites", "Loading sprites...", 0f);
        
        // Load all sprites into dictionary
        string spriteFolderPath = AssetDatabase.GetAssetPath(newSpriteFolder);
        
        // NEW: Search with subfolder support
        string[] searchFolders = scanSubfolders ? 
            new[] { spriteFolderPath } : 
            new[] { spriteFolderPath };
        
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", searchFolders);
        
        Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
        Dictionary<string, string> spritePathDict = new Dictionary<string, string>(); // Track paths
        
        for (int i = 0; i < spriteGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(spriteGuids[i]);
            
            // NEW: Skip if not in subfolder when scanSubfolders is false
            if (!scanSubfolders)
            {
                string dir = Path.GetDirectoryName(path);
                if (dir != spriteFolderPath)
                    continue;
            }
            
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                string spriteName = sprite.name.ToLower().Replace(" ", "").Replace("_", "");
                spriteDict[spriteName] = sprite;
                spritePathDict[spriteName] = path;
                
                // Also store original name
                spriteDict[sprite.name.ToLower()] = sprite;
                spritePathDict[sprite.name.ToLower()] = path;
                
                // Store without any processing for exact match
                spriteDict[sprite.name] = sprite;
                spritePathDict[sprite.name] = path;
            }
            
            EditorUtility.DisplayProgressBar("Scanning Sprites", 
                $"Loading sprite {i + 1}/{spriteGuids.Length}", 
                (float)i / spriteGuids.Length);
        }
        
        Debug.Log($"✅ Loaded {spriteDict.Count} unique sprites from {(scanSubfolders ? "folder and subfolders" : "folder only")}");
        
        // Scan all card data SOs
        EditorUtility.DisplayProgressBar("Scanning Cards", "Loading card data...", 0f);
        
        string cardFolderPath = AssetDatabase.GetAssetPath(cardDataFolder);

// CHỈNH SỬA 1: Tìm kiếm chính xác "t:CardData"
// Điều này hiệu quả hơn là "t:ScriptableObject"
        string[] cardGuids = AssetDatabase.FindAssets("t:CardData", new[] { cardFolderPath });

        for (int i = 0; i < cardGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(cardGuids[i]);

            // CHỈNH SỬA 2: Tải asset về đúng kiểu CardData
            CardData cardData = AssetDatabase.LoadAssetAtPath<CardData>(path);
    
            if (cardData == null) continue;
    
            // CHỈNH SỬA 3: Giờ đây bạn có thể gọi hàm mới
            UpdateItem item = ProcessCardData(cardData, path, spriteDict, spritePathDict);
            updateQueue.Add(item);
    
            EditorUtility.DisplayProgressBar("Scanning Cards", 
                $"Processing card {i + 1}/{cardGuids.Length}", 
                (float)i / cardGuids.Length);
        }

// Đừng quên đóng progress bar sau khi vòng lặp kết thúc
        EditorUtility.ClearProgressBar();
        
        showPreview = true;
        
        int found = updateQueue.Count(u => u.newSprite != null);
        Debug.Log($"✅ Scan complete: {updateQueue.Count} cards, {found} sprites matched, {updateQueue.Count(u => u.willUpdate)} will be updated.");
        
        if (updateQueue.Count(u => u.newSprite == null) > 0)
        {
            Debug.LogWarning($"⚠️ {updateQueue.Count(u => u.newSprite == null)} cards have no matching sprite!");
        }
    }

// Thay đổi quan trọng: Tham số là CardData, không phải ScriptableObject
private UpdateItem ProcessCardData(CardData cardData, string path,
    Dictionary<string, Sprite> spriteDict, Dictionary<string, string> spritePathDict)
{
    UpdateItem item = new UpdateItem();
    item.cardData = cardData; // Gán SO
    
    // --- BẮT ĐẦU PHẦN CHỈNH SỬA ---

    // 1. Lấy tên card trực tiếp từ field 'Name'
    item.cardName = cardData.Name;

    // 2. Dự phòng: Nếu 'Name' bị trống, dùng tên file
    if (string.IsNullOrEmpty(item.cardName))
    {
        item.cardName = Path.GetFileNameWithoutExtension(path);
    }

    // 3. Lấy sprite cũ trực tiếp từ field 'Art'
    item.oldSprite = cardData.Art;
    
    // --- KẾT THÚC PHẦN CHỈNH SỬA ---
    
    // Phần logic tìm kiếm sprite mới của bạn được giữ nguyên
    
    // Find matching sprite
    Sprite foundSprite = null;
    string matchedName = "";
    string foundPath = "";
    
    if (useNameMapper && nameMapper != null)
    {
        // Use name mapper
        List<string> possibleNames = useFuzzyMatch ? 
            nameMapper.GetPossibleSpriteNames(item.cardName) : 
            new List<string> { nameMapper.ConvertToSpriteName(item.cardName) };
        
        item.triedNames = possibleNames;
        
        foreach (string possibleName in possibleNames)
        {
            // Try exact match first
            if (spriteDict.TryGetValue(possibleName, out foundSprite))
            {
                matchedName = possibleName;
                spritePathDict.TryGetValue(possibleName, out foundPath);
                break;
            }
            
            // Try normalized name
            string normalizedName = possibleName.ToLower().Replace(" ", "").Replace("_", "");
            if (spriteDict.TryGetValue(normalizedName, out foundSprite))
            {
                matchedName = possibleName;
                spritePathDict.TryGetValue(normalizedName, out foundPath);
                break;
            }
            
            // Try with original name too
            if (spriteDict.TryGetValue(possibleName.ToLower(), out foundSprite))
            {
                matchedName = possibleName;
                spritePathDict.TryGetValue(possibleName.ToLower(), out foundPath);
                break;
            }
        }
    }
    else
    {
        // Manual pattern
        string searchName = manualPrefix + item.cardName + manualSuffix;
        string normalized = searchName.ToLower().Replace(" ", "").Replace("_", "");
        
        item.triedNames = new List<string> { searchName };
        
        if (spriteDict.TryGetValue(searchName, out foundSprite))
        {
            matchedName = searchName;
            spritePathDict.TryGetValue(searchName, out foundPath);
        }
        else if (spriteDict.TryGetValue(normalized, out foundSprite))
        {
            matchedName = searchName;
            spritePathDict.TryGetValue(normalized, out foundPath);
        }
    }
    
    item.newSprite = foundSprite;
    item.matchedSpriteName = matchedName;
    item.spritePath = foundPath;
    item.willUpdate = foundSprite != null && item.oldSprite != foundSprite;
    
    if (foundSprite != null)
    {
        item.status = item.willUpdate ? "✅ Will Update" : "ℹ️ Same Sprite";
    }
    else
    {
        item.status = "❌ Not Found";
    }
    
    return item;
}
    private void ApplyUpdates()
    {
        int successCount = 0;
        int failCount = 0;
        
        EditorUtility.DisplayProgressBar("Applying Updates", "Updating cards...", 0f);
        
        var itemsToUpdate = updateQueue.Where(u => u.willUpdate).ToList();
        
        for (int i = 0; i < itemsToUpdate.Count; i++)
        {
            var item = itemsToUpdate[i];
            
            try
            {
                var spriteField = item.cardData.GetType().GetField("cardSprite") ?? 
                                 item.cardData.GetType().GetField("sprite") ??
                                 item.cardData.GetType().GetField("icon") ??
                                 item.cardData.GetType().GetField("cardIcon");
                
                if (spriteField != null && item.newSprite != null)
                {
                    spriteField.SetValue(item.cardData, item.newSprite);
                    EditorUtility.SetDirty(item.cardData);
                    successCount++;
                    
                    Debug.Log($"✅ Updated: {item.cardName} → {item.newSprite.name}");
                }
                else
                {
                    failCount++;
                    Debug.LogWarning($"⚠️ Failed: {item.cardName} - No sprite field or sprite found");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Failed to update {item.cardName}: {e.Message}");
                failCount++;
            }
            
            EditorUtility.DisplayProgressBar("Applying Updates", 
                $"Updating {i + 1}/{itemsToUpdate.Count}", 
                (float)i / itemsToUpdate.Count);
        }
        
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        
        string message = $"✅ Update Complete!\n\nSuccess: {successCount}\nFailed: {failCount}";
        EditorUtility.DisplayDialog("Update Complete", message, "OK");
        
        Debug.Log($"════════════════════════════════════════");
        Debug.Log(message);
        Debug.Log($"════════════════════════════════════════");
        
        // Refresh preview
        ScanForUpdates();
    }

    private void CreateNewMapper()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Create Card Name Mapper",
            "CardNameMapper",
            "asset",
            "Chọn nơi lưu CardNameMapper"
        );
        
        if (!string.IsNullOrEmpty(path))
        {
            CardNameMapper newMapper = CreateInstance<CardNameMapper>();
            AssetDatabase.CreateAsset(newMapper, path);
            AssetDatabase.SaveAssets();
            
            nameMapper = newMapper;
            
            EditorUtility.DisplayDialog("Success", "Đã tạo CardNameMapper mới!", "OK");
            EditorGUIUtility.PingObject(newMapper);
        }
    }

    private void TestMapper()
    {
        if (nameMapper == null) return;
        
        // Test with common cards
        string[] testCards = new string[]
        {
            "Ace of Diamonds",
            "Two of Hearts",
            "King of Spades",
            "Queen of Clubs"
        };
        
        Debug.Log("════════════════════════════════════════");
        Debug.Log("🧪 Testing Card Name Mapper");
        Debug.Log("════════════════════════════════════════");
        
        foreach (string testCard in testCards)
        {
            Debug.Log($"\nCard: '{testCard}'");
            Debug.Log($"  Main: {nameMapper.ConvertToSpriteName(testCard)}");
            
            if (useFuzzyMatch)
            {
                var possibilities = nameMapper.GetPossibleSpriteNames(testCard);
                Debug.Log($"  Variations ({possibilities.Count}): {string.Join(", ", possibilities.Take(5))}");
            }
        }
        
        Debug.Log("════════════════════════════════════════");
    }
    
    private void ExportReport()
    {
        string path = EditorUtility.SaveFilePanel(
            "Export Report",
            "",
            $"CardSpriteReport_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt",
            "txt"
        );
        
        if (string.IsNullOrEmpty(path)) return;
        
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("═══════════════════════════════════════════════════════");
                writer.WriteLine("          CARD SPRITE UPDATE REPORT");
                writer.WriteLine("═══════════════════════════════════════════════════════");
                writer.WriteLine($"Generated: {System.DateTime.Now}");
                writer.WriteLine($"Total Cards: {updateQueue.Count}");
                writer.WriteLine($"Will Update: {updateQueue.Count(u => u.willUpdate)}");
                writer.WriteLine($"Not Found: {updateQueue.Count(u => u.newSprite == null)}");
                writer.WriteLine($"No Change: {updateQueue.Count(u => !u.willUpdate && u.newSprite != null)}");
                writer.WriteLine("═══════════════════════════════════════════════════════\n");
                
                // Section 1: Cards to be updated
                writer.WriteLine("\n✅ CARDS TO BE UPDATED:");
                writer.WriteLine("───────────────────────────────────────────────────────");
                var toUpdate = updateQueue.Where(u => u.willUpdate).ToList();
                if (toUpdate.Count > 0)
                {
                    foreach (var item in toUpdate)
                    {
                        writer.WriteLine($"\n• {item.cardName}");
                        writer.WriteLine($"  Old: {(item.oldSprite != null ? item.oldSprite.name : "(none)")}");
                        writer.WriteLine($"  New: {item.newSprite.name}");
                        writer.WriteLine($"  Matched: {item.matchedSpriteName}");
                        if (!string.IsNullOrEmpty(item.spritePath))
                            writer.WriteLine($"  Path: {item.spritePath}");
                    }
                }
                else
                {
                    writer.WriteLine("  (none)");
                }
                
                // Section 2: Cards not found
                writer.WriteLine("\n\n❌ CARDS NOT FOUND:");
                writer.WriteLine("───────────────────────────────────────────────────────");
                var notFound = updateQueue.Where(u => u.newSprite == null).ToList();
                if (notFound.Count > 0)
                {
                    foreach (var item in notFound)
                    {
                        writer.WriteLine($"\n• {item.cardName}");
                        writer.WriteLine($"  Tried names: {string.Join(", ", item.triedNames)}");
                    }
                }
                else
                {
                    writer.WriteLine("  (none)");
                }
                
                // Section 3: Cards with no change
                writer.WriteLine("\n\nℹ️ CARDS WITH NO CHANGE:");
                writer.WriteLine("───────────────────────────────────────────────────────");
                var noChange = updateQueue.Where(u => !u.willUpdate && u.newSprite != null).ToList();
                if (noChange.Count > 0)
                {
                    foreach (var item in noChange)
                    {
                        writer.WriteLine($"• {item.cardName} → {item.newSprite.name}");
                    }
                }
                else
                {
                    writer.WriteLine("  (none)");
                }
                
                writer.WriteLine("\n═══════════════════════════════════════════════════════");
                writer.WriteLine("                    END OF REPORT");
                writer.WriteLine("═══════════════════════════════════════════════════════");
            }
            
            EditorUtility.DisplayDialog("Report Exported", $"Report saved to:\n{path}", "OK");
            Application.OpenURL("file://" + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export report: {e.Message}");
            EditorUtility.DisplayDialog("Export Failed", $"Could not export report:\n{e.Message}", "OK");
        }
    }
}

// Simple input dialog helper
public static class EditorInputDialog
{
    public static string Show(string title, string message, string defaultValue)
    {
        return defaultValue; // Unity doesn't have built-in input dialog, user can test via Inspector
    }
}
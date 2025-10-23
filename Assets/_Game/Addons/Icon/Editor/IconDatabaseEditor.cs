using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(IconDatabase))]
public class IconDatabaseEditor : Editor
{
    private IconDatabase iconDB;
    private Vector2 scrollPosition;
    private int previewSize = 64;
    private bool showPreview = true;
    private string searchFilter = "";

    private void OnEnable()
    {
        iconDB = (IconDatabase)target;
    }

    public override void OnInspectorGUI()
    {
        // Header đẹp hơn
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Icon Database Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Vẽ các settings
        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        // Kiểm tra thư mục có được gán chưa
        bool hasFolder = iconDB.spriteFolder != null;
        
        // Style cho button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 12;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.fixedHeight = 30;

        // Nút tìm và gán sprites
        EditorGUI.BeginDisabledGroup(!hasFolder);
        
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = hasFolder ? new Color(0.4f, 0.8f, 0.4f) : Color.gray;
        
        if (GUILayout.Button("🔍 Find and Assign Sprites", buttonStyle))
        {
            AssignSprites();
        }
        
        GUI.backgroundColor = originalColor;
        EditorGUI.EndDisabledGroup();

        if (!hasFolder)
        {
            EditorGUILayout.HelpBox("⚠️ Vui lòng gán thư mục Sprite Folder trước!", MessageType.Warning);
        }

        // Các nút phụ
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Clear All", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Xác nhận", 
                "Bạn có chắc muốn xóa tất cả sprites?", "Có", "Không"))
            {
                ClearSprites();
            }
        }
        
        if (GUILayout.Button("Remove Duplicates", GUILayout.Height(25)))
        {
            RemoveDuplicates();
        }
        
        if (GUILayout.Button("Sort by Name", GUILayout.Height(25)))
        {
            SortSprites();
        }
        
        EditorGUILayout.EndHorizontal();

        // Preview section
        if (iconDB.icons.Count > 0)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField($"Total Sprites: {iconDB.icons.Count}", EditorStyles.boldLabel);
            
            // Toggle preview
            showPreview = EditorGUILayout.Foldout(showPreview, "Preview", true);
            
            if (showPreview)
            {
                // Search bar
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Search:", GUILayout.Width(50));
                searchFilter = EditorGUILayout.TextField(searchFilter);
                EditorGUILayout.EndHorizontal();

                // Preview size slider
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Size:", GUILayout.Width(50));
                previewSize = EditorGUILayout.IntSlider(previewSize, 32, 128);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(5);

                // Scrollable preview area
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, 
                    GUILayout.Height(300));
                
                DrawSpriteGrid();
                
                EditorGUILayout.EndScrollView();
            }
        }

        // Thông báo nếu có
        if (GUI.changed)
        {
            EditorUtility.SetDirty(iconDB);
        }
    }

    private void DrawSpriteGrid()
    {
        if (iconDB.icons == null || iconDB.icons.Count == 0)
            return;

        // Filter sprites
        var filteredSprites = iconDB.icons.Where(s => 
            string.IsNullOrEmpty(searchFilter) || 
            s.name.ToLower().Contains(searchFilter.ToLower())
        ).ToList();

        if (filteredSprites.Count == 0)
        {
            EditorGUILayout.HelpBox("Không tìm thấy sprite nào phù hợp.", MessageType.Info);
            return;
        }

        int columns = Mathf.Max(1, (int)(EditorGUIUtility.currentViewWidth - 40) / (previewSize + 10));
        int currentColumn = 0;

        EditorGUILayout.BeginHorizontal();
        
        foreach (var sprite in filteredSprites)
        {
            if (sprite == null) continue;

            if (currentColumn >= columns)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                currentColumn = 0;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(previewSize + 10));
            
            // Draw sprite preview
            Rect rect = GUILayoutUtility.GetRect(previewSize, previewSize);
            EditorGUI.DrawPreviewTexture(rect, sprite.texture);
            
            // Draw sprite name
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.fontSize = 9;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.wordWrap = true;
            EditorGUILayout.LabelField(sprite.name, labelStyle, GUILayout.Height(30));
            
            EditorGUILayout.EndVertical();
            currentColumn++;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void AssignSprites()
    {
        if (iconDB.spriteFolder == null)
        {
            Debug.LogWarning("⚠️ Vui lòng gán thư mục (Sprite Folder) trước!");
            return;
        }

        iconDB.icons.Clear();
        string folderPath = AssetDatabase.GetAssetPath(iconDB.spriteFolder);

        // Tùy chọn tìm trong subfolder
        SearchOption searchOption = iconDB.includeSubfolders ? 
            SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

        // Tìm sprites
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

        if (guids.Length == 0)
        {
            Debug.LogWarning($"⚠️ Không tìm thấy Sprite nào trong: {folderPath}");
            EditorUtility.DisplayDialog("Không tìm thấy", 
                "Không có sprite nào trong thư mục đã chọn!", "OK");
            return;
        }

        // Load sprites
        int loadedCount = 0;
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

            if (sprite != null)
            {
                // Filter theo keyword nếu có
                if (!string.IsNullOrEmpty(iconDB.filterKeyword) && 
                    !sprite.name.ToLower().Contains(iconDB.filterKeyword.ToLower()))
                {
                    continue;
                }

                iconDB.icons.Add(sprite);
                loadedCount++;
            }
        }

        // Sắp xếp nếu cần
        if (iconDB.sortByName && iconDB.icons.Count > 0)
        {
            iconDB.icons = iconDB.icons.OrderBy(s => s.name).ToList();
        }

        // Update info
        iconDB.UpdateInfo();
        
        // Save changes
        EditorUtility.SetDirty(iconDB);
        AssetDatabase.SaveAssets();

        // Thông báo
        string message = $"✅ Hoàn tất! Đã load {loadedCount} sprites vào {iconDB.name}";
        Debug.Log(message);
        EditorUtility.DisplayDialog("Thành công", message, "OK");
    }

    private void ClearSprites()
    {
        iconDB.icons.Clear();
        iconDB.UpdateInfo();
        EditorUtility.SetDirty(iconDB);
        AssetDatabase.SaveAssets();
        Debug.Log("🗑️ Đã xóa tất cả sprites");
    }

    private void RemoveDuplicates()
    {
        int originalCount = iconDB.icons.Count;
        iconDB.icons = iconDB.icons.Distinct().ToList();
        int removedCount = originalCount - iconDB.icons.Count;
        
        if (removedCount > 0)
        {
            iconDB.UpdateInfo();
            EditorUtility.SetDirty(iconDB);
            AssetDatabase.SaveAssets();
            Debug.Log($"🧹 Đã xóa {removedCount} sprites trùng lặp");
        }
        else
        {
            Debug.Log("✅ Không có sprites trùng lặp");
        }
    }

    private void SortSprites()
    {
        iconDB.icons = iconDB.icons.OrderBy(s => s.name).ToList();
        iconDB.UpdateInfo();
        EditorUtility.SetDirty(iconDB);
        AssetDatabase.SaveAssets();
        Debug.Log("✅ Đã sắp xếp sprites theo tên");
    }

    private enum SearchOption
    {
        TopDirectoryOnly,
        AllDirectories
    }
}
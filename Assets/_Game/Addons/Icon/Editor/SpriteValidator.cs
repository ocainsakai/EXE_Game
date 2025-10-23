using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

/// <summary>
/// Utility tool để kiểm tra và validate sprites trước khi update
/// </summary>
public class SpriteValidator : EditorWindow
{
    private DefaultAsset spriteFolder;
    private bool scanSubfolders = true;
    private Vector2 scrollPosition;
    private List<SpriteInfo> spriteList = new List<SpriteInfo>();
    private string searchFilter = "";
    
    private class SpriteInfo
    {
        public Sprite sprite;
        public string name;
        public string path;
        public string folder;
        public Vector2 size;
        public long fileSize;
    }
    
    [MenuItem("Tools/Sprite Validator")]
    public static void ShowWindow()
    {
        var window = GetWindow<SpriteValidator>("Sprite Validator");
        window.minSize = new Vector2(500, 600);
    }
    
    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        
        // Header
        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
        headerStyle.fontSize = 16;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("🔍 Sprite Validator & Browser", headerStyle);
        
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "Tool này giúp kiểm tra tất cả sprites trong folder và subfolder",
            MessageType.Info
        );
        
        EditorGUILayout.Space(10);
        
        // Input
        EditorGUILayout.LabelField("📁 Settings", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        
        spriteFolder = EditorGUILayout.ObjectField(
            new GUIContent("Sprite Folder", "Folder chứa sprites cần kiểm tra"),
            spriteFolder,
            typeof(DefaultAsset),
            false
        ) as DefaultAsset;
        
        scanSubfolders = EditorGUILayout.Toggle(
            new GUIContent("Scan Subfolders", "Quét cả subfolder"),
            scanSubfolders
        );
        
        if (EditorGUI.EndChangeCheck())
        {
            spriteList.Clear();
        }
        
        EditorGUILayout.Space(10);
        
        // Scan button
        EditorGUI.BeginDisabledGroup(spriteFolder == null);
        GUI.backgroundColor = new Color(0.4f, 0.7f, 1f);
        if (GUILayout.Button("🔍 Scan Sprites", GUILayout.Height(35)))
        {
            ScanSprites();
        }
        GUI.backgroundColor = Color.white;
        EditorGUI.EndDisabledGroup();
        
        if (spriteFolder == null)
        {
            EditorGUILayout.HelpBox("⚠️ Vui lòng chọn Sprite Folder!", MessageType.Warning);
        }
        
        EditorGUILayout.Space(10);
        
        // Results
        if (spriteList.Count > 0)
        {
            DrawResults();
        }
    }
    
    private void ScanSprites()
    {
        spriteList.Clear();
        
        string folderPath = AssetDatabase.GetAssetPath(spriteFolder);
        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        
        EditorUtility.DisplayProgressBar("Scanning", "Loading sprites...", 0f);
        
        HashSet<string> processedSprites = new HashSet<string>();
        
        for (int i = 0; i < spriteGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(spriteGuids[i]);
            
            // Filter by subfolder setting
            if (!scanSubfolders)
            {
                string dir = Path.GetDirectoryName(path);
                if (dir != folderPath)
                    continue;
            }
            
            // Avoid duplicates from multi-sprite textures
            if (processedSprites.Contains(path))
                continue;
            
            processedSprites.Add(path);
            
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                FileInfo fileInfo = new FileInfo(path);
                
                SpriteInfo info = new SpriteInfo
                {
                    sprite = sprite,
                    name = sprite.name,
                    path = path,
                    folder = Path.GetDirectoryName(path).Replace(folderPath, "").TrimStart('/', '\\'),
                    size = new Vector2(sprite.texture.width, sprite.texture.height),
                    fileSize = fileInfo.Exists ? fileInfo.Length : 0
                };
                
                spriteList.Add(info);
            }
            
            EditorUtility.DisplayProgressBar("Scanning", 
                $"Loading {i + 1}/{spriteGuids.Length}", 
                (float)i / spriteGuids.Length);
        }
        
        EditorUtility.ClearProgressBar();
        
        // Sort by name
        spriteList = spriteList.OrderBy(s => s.name).ToList();
        
        Debug.Log($"✅ Found {spriteList.Count} sprites in {(scanSubfolders ? "folder and subfolders" : "folder only")}");
        
        // Analyze duplicates
        var duplicates = spriteList.GroupBy(s => s.name.ToLower())
            .Where(g => g.Count() > 1)
            .ToList();
        
        if (duplicates.Any())
        {
            Debug.LogWarning($"⚠️ Found {duplicates.Count} duplicate sprite names!");
            foreach (var dup in duplicates)
            {
                Debug.LogWarning($"  • '{dup.Key}' appears {dup.Count()} times");
            }
        }
    }
    
    private void DrawResults()
    {
        EditorGUILayout.LabelField($"📋 Results ({spriteList.Count} sprites)", EditorStyles.boldLabel);
        
        // Statistics
        EditorGUILayout.BeginHorizontal();
        
        int uniqueFolders = spriteList.Select(s => s.folder).Distinct().Count();
        long totalSize = spriteList.Sum(s => s.fileSize);
        
        EditorGUILayout.HelpBox($"Folders: {uniqueFolders}", MessageType.None);
        EditorGUILayout.HelpBox($"Total Size: {FormatFileSize(totalSize)}", MessageType.None);
        EditorGUILayout.HelpBox($"Avg: {spriteList.Average(s => s.size.x):F0}x{spriteList.Average(s => s.size.y):F0}", MessageType.None);
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Search filter
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search:", GUILayout.Width(60));
        searchFilter = EditorGUILayout.TextField(searchFilter);
        if (GUILayout.Button("Clear", GUILayout.Width(60)))
        {
            searchFilter = "";
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Export buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("📄 Export List", GUILayout.Height(25)))
        {
            ExportSpriteList();
        }
        if (GUILayout.Button("📊 Export CSV", GUILayout.Height(25)))
        {
            ExportCSV();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Scrollable list
        var filteredList = string.IsNullOrEmpty(searchFilter) ? 
            spriteList : 
            spriteList.Where(s => s.name.ToLower().Contains(searchFilter.ToLower())).ToList();
        
        EditorGUILayout.LabelField($"Showing {filteredList.Count} of {spriteList.Count}", EditorStyles.miniLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(350));
        
        foreach (var info in filteredList)
        {
            DrawSpriteItem(info);
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    private void DrawSpriteItem(SpriteInfo info)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        
        // Preview
        Rect previewRect = GUILayoutUtility.GetRect(40, 40);
        EditorGUI.DrawPreviewTexture(previewRect, info.sprite.texture);
        
        // Info
        EditorGUILayout.BeginVertical();
        
        EditorGUILayout.LabelField(info.name, EditorStyles.boldLabel);
        
        GUIStyle miniStyle = new GUIStyle(EditorStyles.miniLabel);
        miniStyle.normal.textColor = Color.gray;
        
        if (!string.IsNullOrEmpty(info.folder))
        {
            EditorGUILayout.LabelField($"📁 {info.folder}", miniStyle);
        }
        
        EditorGUILayout.LabelField(
            $"📐 {info.size.x}x{info.size.y} | 💾 {FormatFileSize(info.fileSize)}", 
            miniStyle
        );
        
        EditorGUILayout.EndVertical();
        
        // Buttons
        if (GUILayout.Button("Ping", GUILayout.Width(50)))
        {
            EditorGUIUtility.PingObject(info.sprite);
        }
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(2);
    }
    
    private void ExportSpriteList()
    {
        string path = EditorUtility.SaveFilePanel(
            "Export Sprite List",
            "",
            $"SpriteList_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt",
            "txt"
        );
        
        if (string.IsNullOrEmpty(path)) return;
        
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("═══════════════════════════════════════════════════════");
                writer.WriteLine("              SPRITE LIST REPORT");
                writer.WriteLine("═══════════════════════════════════════════════════════");
                writer.WriteLine($"Generated: {System.DateTime.Now}");
                writer.WriteLine($"Total Sprites: {spriteList.Count}");
                writer.WriteLine($"Scan Subfolders: {scanSubfolders}");
                writer.WriteLine("═══════════════════════════════════════════════════════\n");
                
                // Group by folder
                var grouped = spriteList.GroupBy(s => s.folder).OrderBy(g => g.Key);
                
                foreach (var group in grouped)
                {
                    string folderName = string.IsNullOrEmpty(group.Key) ? "(Root)" : group.Key;
                    writer.WriteLine($"\n📁 {folderName} ({group.Count()} sprites)");
                    writer.WriteLine("───────────────────────────────────────────────────────");
                    
                    foreach (var sprite in group.OrderBy(s => s.name))
                    {
                        writer.WriteLine($"  • {sprite.name}");
                        writer.WriteLine($"    Size: {sprite.size.x}x{sprite.size.y} | {FormatFileSize(sprite.fileSize)}");
                    }
                }
                
                writer.WriteLine("\n═══════════════════════════════════════════════════════");
            }
            
            EditorUtility.DisplayDialog("Export Complete", $"Sprite list saved to:\n{path}", "OK");
            Application.OpenURL("file://" + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export: {e.Message}");
        }
    }
    
    private void ExportCSV()
    {
        string path = EditorUtility.SaveFilePanel(
            "Export Sprite CSV",
            "",
            $"Sprites_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv",
            "csv"
        );
        
        if (string.IsNullOrEmpty(path)) return;
        
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Header
                writer.WriteLine("Name,Folder,Width,Height,FileSize,Path");
                
                // Data
                foreach (var sprite in spriteList)
                {
                    writer.WriteLine($"\"{sprite.name}\",\"{sprite.folder}\",{sprite.size.x},{sprite.size.y},{sprite.fileSize},\"{sprite.path}\"");
                }
            }
            
            EditorUtility.DisplayDialog("Export Complete", $"CSV saved to:\n{path}", "OK");
            Application.OpenURL("file://" + path);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to export CSV: {e.Message}");
        }
    }
    
    private string FormatFileSize(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";
        else if (bytes < 1024 * 1024)
            return $"{bytes / 1024} KB";
        else
            return $"{bytes / (1024 * 1024)} MB";
    }
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIconDatabase", menuName = "MyGame/Icon Database")]
public class IconDatabase : ScriptableObject
{
    [Header("Settings")]
    [Tooltip("Kéo thả thư mục chứa sprites vào đây")]
    public UnityEditor.DefaultAsset spriteFolder;
    
    [Tooltip("Tìm sprites trong cả thư mục con")]
    public bool includeSubfolders = true;
    
    [Tooltip("Sắp xếp sprites theo tên")]
    public bool sortByName = true;
    
    [Tooltip("Chỉ load sprites có tên chứa từ khóa này (để trống = load tất cả)")]
    public string filterKeyword = "";

    [Header("Runtime Data (Auto-filled)")]
    public List<Sprite> icons = new List<Sprite>();
    
    [Header("Info")]
    [SerializeField] private int totalCount = 0;
    [SerializeField] private string lastUpdateTime = "";

    public void UpdateInfo()
    {
        totalCount = icons.Count;
        lastUpdateTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }

    // Helper method để tìm sprite theo tên
    public Sprite GetSpriteByName(string spriteName)
    {
        return icons.Find(s => s.name == spriteName);
    }

    // Helper method để lấy sprite theo index
    public Sprite GetSpriteByIndex(int index)
    {
        if (index >= 0 && index < icons.Count)
            return icons[index];
        return null;
    }
}
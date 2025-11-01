using _Game.Addons.Deck.Scripts;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(CardData))]
public class CardDataEditor : Editor
{
    private const float PREVIEW_SIZE = 150f;

    public override void OnInspectorGUI()
    {
        // Vẽ các trường mặc định
        DrawDefaultInspector();

        // Lấy reference đến CardData
        CardData cardData = (CardData)target;

        // Nếu có Art sprite, hiển thị preview
        if (cardData.Art != null)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Art Preview", EditorStyles.boldLabel);
            
            // Tạo một rect cho preview
            Rect previewRect = GUILayoutUtility.GetRect(PREVIEW_SIZE, PREVIEW_SIZE, GUILayout.ExpandWidth(false));
            
            // Vẽ background
            EditorGUI.DrawRect(previewRect, new Color(0.2f, 0.2f, 0.2f, 1f));
            
            // Vẽ sprite
            Texture2D texture = AssetPreview.GetAssetPreview(cardData.Art);
            if (texture != null)
            {
                GUI.DrawTexture(previewRect, texture, ScaleMode.ScaleToFit);
            }
            
            // Hiển thị thông tin sprite
            EditorGUILayout.LabelField("Size:", $"{cardData.Art.rect.width} x {cardData.Art.rect.height}");
            EditorGUILayout.LabelField("Texture:", cardData.Art.texture.name);
        }
        else
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("No Art sprite assigned. Assign a sprite to see preview.", MessageType.Info);
        }
    }
}
#endif
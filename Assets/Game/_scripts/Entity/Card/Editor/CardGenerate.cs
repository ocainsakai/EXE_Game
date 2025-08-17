using System.IO;
using UnityEditor;
using UnityEngine;

public class CardGenerate : EditorWindow
{
    [MenuItem("Tools/Generate Deck")]
    public static void ShowWindow()
    {
        GetWindow<CardGenerate>("Card Generator");
    }
    private void OnGUI()
    {
        GUILayout.Label("Card Generator", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Deck"))
        {
            GenerateDeck();
        }
    }
#if UNITY_EDITOR
    public static string SpriteFolderPath = "Assets/Resources/Art/PNG";
    public static string CardDataFolderPath = "Assets/GameData/Deck";
    public static void GenerateDeck()
    {
        if (!Directory.Exists(CardDataFolderPath))
        {
            Directory.CreateDirectory(CardDataFolderPath);
        }
        for (int suit = 0; suit < 4; suit++)
        {
            for (int rank = 2; rank < 15; rank++)
            {
                CardSDData newCard = CreateInstance<CardSDData>();
                newCard.Suit = suit;
                newCard.Rank = rank;

                var _rank = rank == 14 ? 1 : rank;
                newCard.name = $"card-{newCard.GetSuit().ToLower()}-{_rank}";
                newCard.Art = AssetDatabase.LoadAssetAtPath<Sprite>(
                    $"{SpriteFolderPath}/{newCard.name}.png");

                string filePath = $"{CardDataFolderPath}/{newCard.name}.asset";
                filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

                AssetDatabase.CreateAsset(newCard, filePath);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}

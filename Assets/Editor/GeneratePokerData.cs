using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class GeneratePokerTypeData : EditorWindow
{
    [MenuItem("Tools/Generate Poker")]
    public static void ShowWindow()
    {
        GetWindow<GeneratePokerTypeData>("Card Generator");
    }
    private void OnGUI()
    {
        GUILayout.Label("Poker Generator", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate All PokerTypeData"))
        {
            GeneratePokerAssets();
        }
    }
    public static string folderPath = "Assets/Resources/GameData/Poker";

    private static void GeneratePokerAssets()
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        List<(PokerType type, int chip, int mult)> pokerHandConfigs = new()
        {
            (PokerType.PokerType, 0, 0),
            (PokerType.HighCard,      5,   1),
            (PokerType.Pair,          10,  2),
            (PokerType.TwoPair,       20,  2),
            (PokerType.ThreeOfAKind,  30,  3),
            (PokerType.Straight,      30,  4),
            (PokerType.Flush,         35,  4),
            (PokerType.FullHouse,     40,  4),
            (PokerType.FourOfAKind,   60,  7),
            (PokerType.StraightFlush, 100, 8),
            (PokerType.FiveOfAKind,   120, 12),
            (PokerType.FlushHouse,    140, 14),
            (PokerType.FlushFive,     160, 16)
        };

        foreach (var config in pokerHandConfigs)
        {
            PokerTypeData pokerAsset = CreatePokerTypeData(config.type, config.chip, config.mult);

            string path = $"{folderPath}/{pokerAsset.Type}.asset";
            AssetDatabase.CreateAsset(pokerAsset, path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("PokerTypeData assets generated successfully.");
    }

    private static PokerTypeData CreatePokerTypeData(PokerType handType, int chip, int mult)
    {
        PokerTypeData asset = ScriptableObject.CreateInstance<PokerTypeData>();
        asset.Type = handType;
        asset.BaseChip = chip;
        asset.BaseMult = mult;
        return asset;
    }
}

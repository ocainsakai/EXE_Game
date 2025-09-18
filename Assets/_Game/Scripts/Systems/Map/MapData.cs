using Map;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Map/Map Data")]
public class MapData : ScriptableObject
{
    [Serializable]
    public class HexEntry
    {
        public Vector2Int position;
        public HexContentType type;
    }

    [SerializeField] private List<HexEntry> entries = new();
    public IReadOnlyList<HexEntry> Entries => entries;
    public IReadOnlyList<Vector2Int> Positions
    {
        get
        {
            var positions = new List<Vector2Int>();
            foreach (var entry in entries)
            {
                positions.Add(entry.position);
            }
            return positions;
        }
    }
    public IReadOnlyList<HexContentType> Types
    {
        get
        {
            var types = new List<HexContentType>();
            foreach (var entry in entries)
            {
                types.Add(entry.type);
            }
            return types;
        }
    }
    public Dictionary<Vector2Int, HexContentType> ToDictionary()
    {
        var dict = new Dictionary<Vector2Int, HexContentType>();
        foreach (var entry in entries)
        {
            if (!dict.ContainsKey(entry.position))
                dict.Add(entry.position, entry.type);
        }
        return dict;
    }


#if UNITY_EDITOR
    [ContextMenu("Create Default Data")]
    public void CreateDefault()
    {
        entries.Clear();

        Vector2Int[] positions =
        {
            new(0,0), new(0,1), new(-1,1), new(-1,2), new(0,2), new(1,2),
            new(0,3), new(-1,3), new(-1,4), new(0,4), new(1,4), new(0,5),
            new(-1,5), new(-1,6), new(0,6), new(1,6), new(0,7), new(-1,7),
            new(-1,8), new(0,8), new(1,8), new(0,9), new(-1,9), new(0,10)
        };

        HexContentType[] types =
        {
            HexContentType.Player, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy,
            HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy,
            HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy,
            HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy,
            HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy,
            HexContentType.Enemy, HexContentType.Enemy, HexContentType.Enemy, HexContentType.Boss
        };

        for (int i = 0; i < positions.Length; i++)
        {
            entries.Add(new HexEntry { position = positions[i], type = types[i] });
        }

        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log("Default MapData created with " + entries.Count + " entries.");
    }
#endif
}

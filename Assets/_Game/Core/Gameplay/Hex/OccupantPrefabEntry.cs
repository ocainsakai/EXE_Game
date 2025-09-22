using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OccupantPrefabEntry", menuName = "Map/OccupantPrefabEntry")]
public class OccupantPrefabEntry : ScriptableObject
{
    [Serializable]
    public class HexContentTypeList
    {
        public HexContentType type;
        public List<GameObject> prefabs = new();
    }
    public List<HexContentTypeList> occupantPrefabs = new();
}

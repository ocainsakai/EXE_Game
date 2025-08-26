using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public enum HexType
    {
        Player,
        Fight,
        Shop,
        Treasure,
        Heal,
        Boss
    }
    public class MapGenerator : MonoBehaviour
    {
        [Header("Map Visual")]
        [SerializeField] public Tilemap tilemap;
        [SerializeField] private Tile hexTile;

        [SerializeField]
        private List<GameObject> initObjects;
        public readonly List<Vector2Int> mapPosition = new()
        {
            new Vector2Int(0, 0),    // Player start
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 2),
            new Vector2Int(0, 2),
            new Vector2Int(1, 2),
            new Vector2Int(0, 3),
            new Vector2Int(-1, 3),
            new Vector2Int(-1, 4),
            new Vector2Int(0, 4),
            new Vector2Int(1, 4),
            new Vector2Int(0, 5),
            new Vector2Int(-1, 5),
            new Vector2Int(-1, 6),
            new Vector2Int(0, 6),
            new Vector2Int(1, 6),
            new Vector2Int(0, 7),
            new Vector2Int(-1, 7),
            new Vector2Int(-1, 8),
            new Vector2Int(0, 8),
            new Vector2Int(1, 8),
            new Vector2Int(0, 9),
            new Vector2Int(-1, 9),
            new Vector2Int(0, 10),   // Boss end
        };
        public readonly List<HexType> mapInitTypes = new()
        {
            HexType.Player,    // Player start
            HexType.Fight,
            HexType.Shop,
            HexType.Fight,
            HexType.Treasure,
            HexType.Fight,
            HexType.Heal,
            HexType.Fight,
            HexType.Shop,
            HexType.Fight,
            HexType.Treasure,
            HexType.Fight,
            HexType.Heal,
            HexType.Fight,
            HexType.Shop,
            HexType.Fight,
            HexType.Treasure,
            HexType.Fight,
            HexType.Heal,
            HexType.Fight,
            HexType.Shop,
            HexType.Fight,
            HexType.Fight,
            HexType.Boss,   // Boss end
        };
        public List<GameObject> CreatedObjects { get; private set; } = new List<GameObject>();
        private void Start()
        {
            CreateFloor();
            CreatedObjects = CreateInitObject();
        }
        private void CreateFloor()
        {
            tilemap.ClearAllTiles();
            foreach (var pos in mapPosition)
            {
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), hexTile);
            }
        }

        // need position and type, then set data
        private List<GameObject> CreateInitObject()
        {
            List<GameObject> createdObjects = new List<GameObject>();
            for (int i = 0; i < mapInitTypes.Count; i++)
            {
                var type = mapInitTypes[i];
                var prefab = GetInitObject(mapInitTypes[i]);
                var position = mapPosition[i];
                var worldPos = tilemap.CellToWorld(new Vector3Int(position.x, position.y, 0));

                if(prefab != null)
                {
                    var go = Instantiate(prefab, worldPos, Quaternion.identity, this.transform);
                    var hex = go.GetComponent<HexDataHolder>();
                    hex.SetData(new HexRuntime(position, type));
                    createdObjects.Add(go);
                }
            }
            return createdObjects;
        }
        private GameObject GetInitObject(HexType type)
        {
            foreach(var obj in initObjects)
            {
                if(obj.name.Contains(type.ToString()))
                {
                    return obj;
                }
            }
            return null;
        }
    }
}

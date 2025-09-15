using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Map
{
    public enum HexType
    {
        None,
        Nothing,
        Fight,
        Shop,
        Treasure,
        Heal,
        Boss
    }
    public class MapGrid : MonoBehaviour
    {
        [Header("Map Visual")]
        [SerializeField] public Tilemap tilemap;
        [SerializeField] private Tile hexTile;
        [SerializeField] private List<GameObject> initObjects;

        public Dictionary<Vector2Int ,HexController> hexObjects = new();
        public Dictionary<Vector2Int, HexState> mapStates = new();
        #region DATA
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
    HexType.Nothing,    // Player start
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
        #endregion
        private void Awake()
        {
            if (tilemap == null)
            {
                tilemap = FindFirstObjectByType<Tilemap>();
            }
        }

        public Vector3 GetWorldPos(Vector2Int pos)
        {
            return tilemap.GetCellCenterWorld((Vector3Int)pos);
        }
        public void RenderMap(List<HexState> states, MapManager mapManager)
        {
            ClearMap();
            foreach (var state in states)
            {
                mapStates.Add(state.position, state);
                // Vẽ tile
                tilemap.SetTile((Vector3Int)state.position, state.Type == HexType.None ? null : hexTile);
                CreateHex(state, mapManager);
            }
        }

        public void ClearMap()
        {
            tilemap.ClearAllTiles();
            foreach (var hex in hexObjects)
            {
                if (hex.Value != null) Destroy(hex.Value.gameObject);
            }
            hexObjects.Clear();
            mapStates.Clear();
        }
        public HexState GetState(Vector2Int pos)
        {
            if (mapStates.TryGetValue(pos, out var hex))
                return hex;
            return null;
        }
        public HexController GetHex(Vector2Int pos)
        {
            if (hexObjects.TryGetValue(pos, out var hex))
                return hex;
            return null;
        }
        public List<HexController> GetColumns(int col)
        {
            var list = new List<HexController>();
            foreach (var hex in GetColumnsPos(col))
            {
                list.Add(GetHex(hex));
            }
            return list;
        }
        public List<Vector2Int> GetColumnsPos(int col)
        {
            return mapPosition.Where(x => x.y == col).ToList();
        }

        private HexController CreateHex(HexState state, MapManager mapManager)
        {
            var prefab = initObjects.Find(x => x.name == state.Type.ToString());
            if (prefab == null) return null;
            var go = Instantiate(prefab, tilemap.GetCellCenterWorld((Vector3Int)state.position), Quaternion.identity);
            var controller = go.GetComponent<HexController>();
            hexObjects.Add(state.position,controller);
            controller.position = state.position;
            return controller;
        }
        public void SetNothing(Vector2Int position)
        {
            RemoveObject(position);
            var state = GetState(position);
            state.Type = HexType.Nothing;
        }

        private void RemoveObject(Vector2Int position)
        {
            var go = GetHex(position);
            if (go != null)
            {
                go.gameObject.SetActive(false);
                hexObjects.Remove(position);
            }
        }

        public void SetNone(Vector2Int position)
        {
            RemoveObject(position);
            var state = GetState(position);
            state.Type = HexType.None;
            tilemap.SetTile((Vector3Int)position, null);
        }

        public void SwapState(Vector2Int playerPosition, Vector2Int lastPosition)
        {
            SetNothing(lastPosition);
            var temp = mapStates[lastPosition];
            var temp2 = hexObjects[lastPosition];
            mapStates[lastPosition] = mapStates[playerPosition];
            hexObjects[lastPosition] = hexObjects[playerPosition];
            mapStates[playerPosition] = temp;
            hexObjects[playerPosition] = temp2;
        }
    }
}

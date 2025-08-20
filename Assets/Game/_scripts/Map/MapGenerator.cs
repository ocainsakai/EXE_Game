using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public enum HexType
    {
        Normal,
        Enemy,
        Shop,
        Boss
    }
    public class MapGenerator : MonoBehaviour
    {
        [Header("Map Visual")]
        [SerializeField] public Tilemap tilemap;
        [SerializeField] public Tilemap hoverMap;
        [SerializeField] private Tile hoverTile;

        [Header("Map Data")]
        [SerializeField] public HexData[] Tiles;


        
        private readonly Dictionary<Vector2Int, HexType> hexMap = new()
        {
            [new Vector2Int(0, 0)] = HexType.Normal,   // Player start
            [new Vector2Int(0, 1)] = HexType.Enemy,
            [new Vector2Int(-1, 1)] = HexType.Enemy,
            [new Vector2Int(-1, 2)] = HexType.Enemy,
            [new Vector2Int(0, 2)] = HexType.Shop,
            [new Vector2Int(1, 2)] = HexType.Enemy,
            [new Vector2Int(0, 3)] = HexType.Enemy,
            [new Vector2Int(-1, 3)] = HexType.Enemy,
            [new Vector2Int(-1, 4)] = HexType.Enemy,
            [new Vector2Int(0, 4)] = HexType.Enemy,
            [new Vector2Int(1, 4)] = HexType.Enemy,
            [new Vector2Int(0, 5)] = HexType.Enemy,
            [new Vector2Int(-1, 5)] = HexType.Shop,
            [new Vector2Int(-1, 6)] = HexType.Enemy,
            [new Vector2Int(0, 6)] = HexType.Enemy,
            [new Vector2Int(1, 6)] = HexType.Enemy,
            [new Vector2Int(0, 7)] = HexType.Enemy,
            [new Vector2Int(-1, 7)] = HexType.Enemy,
            [new Vector2Int(-1, 8)] = HexType.Enemy,
            [new Vector2Int(0, 8)] = HexType.Enemy,
            [new Vector2Int(1, 8)] = HexType.Shop,
            [new Vector2Int(0, 9)] = HexType.Enemy,
            [new Vector2Int(-1, 9)] = HexType.Enemy,
            [new Vector2Int(0, 10)] = HexType.Boss,     // Boss end
        };

        public List<HexState> CreateMap()
        {
            var mapData = new List<HexState>();
            foreach (var item in hexMap)
            {
                HexData tileData = null;
                HexType hexType = item.Value;
                tileData = Tiles.FirstOrDefault(x => x.Type == hexType);
                tilemap.SetTile((Vector3Int)item.Key, tileData?.Tile);
                mapData.Add(new HexState(item.Key, tileData));
            }

            return mapData;
        }
        public void Initialized()
        {
            tilemap.ClearAllTiles();
            hoverMap.ClearAllTiles();
        }
       
        //private void LockColomns(int y)
        //{
        //    var tiles = hexMap.Where(x => x.y <= y && x != playerPosition);
        //    foreach (var tilePos in tiles)
        //    {
        //        var tile = mapData.GetHexState(tilePos);
        //        tile.IsLocked = true;
        //        hoverMap.SetTile((Vector3Int)tilePos, hoverTile);
        //    }
        //}
    }
}

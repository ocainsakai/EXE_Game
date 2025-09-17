using System.Collections.Generic;
using UnityEngine;


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
        [SerializeField] private List<GameObject> initObjects;
        public Dictionary<Vector2Int ,HexController> hexObjects = new();
        public Dictionary<Vector2Int, HexState> mapStates = new();

        public void RenderMap(List<HexState> states, MapManager mapManager, Transform container)
        {
            ClearMap();
            foreach (var state in states)
            {
                mapStates.Add(state.position, state);
                // Vẽ tile
            }
        }

        public void ClearMap()
        {
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
        //public List<HexController> GetColumns(int col)
        //{
        //    var list = new List<HexController>();
        //    foreach (var hex in GetColumnsPos(col))
        //    {
        //        list.Add(GetHex(hex));
        //    }
        //    return list;
        //}
        //public List<Vector2Int> GetColumnsPos(int col)
        //{
        //    return mapPosition.Where(x => x.y == col).ToList();
        //}

      
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

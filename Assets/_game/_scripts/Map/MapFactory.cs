using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    
    public static class MapFactory
    {
            // Map mặc định (hardcode như hiện tại)
        public static List<HexState> CreateDefaultMap(List<Vector2Int> positions, List<HexType> types)
        {
            var list = new List<HexState>();
            for (int i = 0; i < positions.Count; i++)
            {
                list.Add(new HexState(types[i], positions[i]));
            }
            return list;
        }

        // Map từ save state
        public static List<HexState> CreateFromState(List<HexState> savedStates)
        {
            return new List<HexState>(savedStates);
        }

        // Map random (demo)
        public static List<HexState> CreateRandom(List<Vector2Int> positions)
        {
            var list = new List<HexState>();
            foreach (var pos in positions)
            {
                var type = (HexType)Random.Range(0, System.Enum.GetValues(typeof(HexType)).Length);
            list.Add(new HexState(type, pos));
            }
            return list;
        }
    }
}

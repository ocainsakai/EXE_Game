using System;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class HexState
    { 
        public bool isRevealed = false;
        public bool isEntering = false;
        public HexType Type;
        public Vector2Int position;
        public HexState(HexType type, Vector2Int pos)
        {
            Type = type;
            position = pos;
        }
    }
}

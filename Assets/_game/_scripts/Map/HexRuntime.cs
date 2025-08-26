using System;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class HexRuntime { 
        public bool isRevealed = false;
        // not destroyed when entering
        public bool isEntering = false;
        public HexType Type;
        public Vector2Int position;
        public HexRuntime(Vector2Int position, HexType type)
        {
            this.position = position;
            this.Type = type;   
        }
    }
}

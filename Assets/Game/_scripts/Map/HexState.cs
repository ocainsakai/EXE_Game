using System;
using UnityEngine;

namespace Map
{
    [Serializable]
    public struct HexState { 
        public bool IsLocked;
        public Vector2Int position;
        public HexData Data;
        public HexState(Vector2Int position, HexData data)
        {
            this.position = position;
            Data = data;
            IsLocked = false;
        }
    }
}

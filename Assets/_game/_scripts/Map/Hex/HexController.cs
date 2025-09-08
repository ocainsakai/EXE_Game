using System;
using UnityEngine;

namespace Map
{
    public class HexController : MonoBehaviour
    {
        public MapManager mapManager;
        public Vector2Int position;
        public void OnMouseDown()
        {
            if (!MapUI.IsBlocking)
            {
                mapManager.OnHexClicked(position);
            }
        }
    }
}
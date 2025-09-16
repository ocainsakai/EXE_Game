using System;
using UnityEngine;

namespace Map
{
    public class HexController : MonoBehaviour
    {
        public static Action<Vector2Int> OnHexClicked;
        public Vector2Int position;
        public void OnMouseDown()
        {
            if (!MapUI.IsBlocking)
            {
                OnHexClicked?.Invoke(position);
            }
        }
    }
}
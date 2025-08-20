using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Map
{
    public class Player : MonoBehaviour
    {
        [SerializeField] HexMapManager hexMapManager;


        private void Start()
        {
            TilemapSelector.OnTileSelected.AddListener(OnTileSelected);
            OnTileSelected(Vector2Int.zero);
        }

        private async void OnTileSelected(Vector2Int arg0)
        {
            await UniTask.Yield();
            transform.position = hexMapManager.GetPlayerPostion();
        }
    }
}
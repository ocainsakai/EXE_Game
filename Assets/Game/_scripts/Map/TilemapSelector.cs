using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Map
{
    public class TilemapSelector : MonoBehaviour, IPointerClickHandler
    {
        public static UnityEvent<Vector2Int> OnTileSelected = new UnityEvent<Vector2Int>();
        [SerializeField] Tilemap tilemap;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Mouse clicked on TilemapSelector");
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (tilemap != null)
            {
                Vector2Int cellPosition = (Vector2Int)tilemap.WorldToCell(mousePosition);
                OnTileSelected?.Invoke(cellPosition);
            }
        }

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }
    }
}
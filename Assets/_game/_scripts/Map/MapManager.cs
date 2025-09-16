using System;
using System.Linq;
using UnityEngine;
using UnityUtils;
using VContainer;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameStates mapState;
        private MapGrid mapGrid;
        private MapUI mapUI;
        private MapMoving mapMoving;
        [Inject] private EnemyManager enemyManager;
        public bool IsTestMode = false;
      
        private void InitMapService()
        {
            if (mapGrid.OrNull() == null)
                mapGrid = GetComponentInChildren<MapGrid>(true);

            if (mapUI.OrNull() == null)
                mapUI = GetComponentInChildren<MapUI>(true);

            if (mapMoving.OrNull() == null)
                mapMoving = GetComponentInChildren<MapMoving>(true);
        }
        [ContextMenu("Init New Game")]
        public void InitNew()
        {
            InitMapService();
            var defaultStates = MapFactory.CreateDefaultMap(mapGrid.mapPosition, mapGrid.mapInitTypes);
            mapGrid.RenderMap(defaultStates, this);
            mapState.mapStates = mapGrid.mapStates;
            mapMoving.SetPlayerPosition(Vector2Int.zero, mapGrid.GetWorldPos(Vector2Int.zero));
            
            // register
            HexController.OnHexClicked += HexClickHandle;
        }

        private void HexClickHandle(Vector2Int position)
        {
            bool isValue = mapMoving.OnHexClicked(position);
            if (isValue)
            {
                // show pop up
                mapUI.ShowPopup(isValue,() => mapMoving.MoveTo(position, mapGrid));
            }
            else
            {
                // show message
                mapUI.ShowMessage(isValue);
            }
        }

        public void LoadFromSave()
        {
            
            var loadedStates = MapFactory.CreateFromState(mapState.mapStates.Values.ToList());
            if (loadedStates.Count == 0)
            {
                InitNew();
                return;
            }
            mapGrid.RenderMap(loadedStates, this);
            mapGrid.mapStates = mapState.mapStates;
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }
    }
}


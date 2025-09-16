using System.Linq;
using UnityEngine;
using UnityUtils;
using VContainer;

namespace Map
{
    public class MapManager :  BaseManager
    {
        [SerializeField] private GameStates mapState;
        [SerializeField] private Transform container;
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private MapUI mapUI;
        [SerializeField] private MapMoving mapMoving;
        [Inject] private EnemyManager enemyManager;
        public bool IsTestMode = false;
      
        [ContextMenu("Init New Game")]
        public override void Init()
        {
            Debug.Log("Init Map Container" + (container.OrNull() == null) + (container));
            Debug.Log("Init Map Grip" + (mapGrid.OrNull() == null) + (mapGrid));
            //InitMapService();
            var defaultStates = MapFactory.CreateDefaultMap(mapGrid.mapPosition, mapGrid.mapInitTypes);
            mapGrid.RenderMap(defaultStates, this, container);
            mapState.mapStates = mapGrid.mapStates;
            mapMoving.SetPlayerPosition(Vector2Int.zero, mapGrid.GetWorldPos(Vector2Int.zero), container);
            
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
                Init();
                return;
            }
            mapGrid.RenderMap(loadedStates, this, container);
            mapGrid.mapStates = mapState.mapStates;
        }
        private void OnDestroy()
        {
            HexController.OnHexClicked -= HexClickHandle;
        }
    }
}


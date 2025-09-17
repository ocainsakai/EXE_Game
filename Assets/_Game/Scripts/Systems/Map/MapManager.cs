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
        [SerializeField] private HexManager mapGrid;
        [SerializeField] private MapUI mapUI;
        [SerializeField] private MapMoving mapMoving;
        [SerializeField] private MapData mapData;
        [Inject] private EnemyManager enemyManager;
        public bool IsTestMode = false;
      
        [ContextMenu("Init New Game")]
        public override void Init()
        {
            mapGrid.GenerateGrid(HexLayout.DataDriven(mapData.Positions));
            // register
            HexController.OnHexClicked += HexClickHandle;
        }

        private void HexClickHandle(Vector2Int position)
        {
            bool isValue = mapMoving.OnHexClicked(position);
            if (isValue)
            {
                // show pop up
                //mapUI.ShowPopup(isValue,() => mapMoving.MoveTo(position, mapGrid));
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
            //mapGrid.RenderMap(loadedStates, this, container);
            //mapGrid.mapStates = mapState.mapStates;
        }
        private void OnDestroy()
        {
            HexController.OnHexClicked -= HexClickHandle;
        }
    }
}


using UnityEngine;

namespace Map
{
    public class MapManager :  BaseManager<MapManager>
    {
        [SerializeField] private HexManager grid;
        [SerializeField] private OccupantFactory occupantFactory;
        [SerializeField] private MapData mapData;
        public bool IsTestMode = false;
      

        private Vector2Int lastClickedHex = new(-999, -999);
        private Vector2Int playerGridPos = new Vector2Int();
        [ContextMenu("Init New Game")]
        public override void Init()
        {
            grid.GenerateGrid(HexLayout.DataDriven(mapData.Positions));
            foreach (var occupant in mapData.Entries)
            {
                var hex = grid.GetHexAt(occupant.position);
                if (hex == null) continue;
                if (occupant.type == HexContentType.Player)
                    playerGridPos = occupant.position;
                var instance = occupantFactory.CreateOccupant(occupant.type, grid.transform, hex.transform.position);
                instance?.SetHex(hex);
            }
        }
        public void OnClickHandle(Vector2Int gridPos, object data = null)
        {
            var hex = grid.GetHexAt(gridPos);
            if (hex == null) return;
            if (grid.IsNeighborRightOf(playerGridPos, gridPos))
            {
                // show popup with action choices
                UIManager.Instance.GetType<MapPopup>().Show(data, null, () => EnterTheBattle());

            }
            else
            {
                UIManager.Instance.GetType<MapPopup>().Show(data);

            }
        }

        public void EnterTheBattle()
        {
            RunManager.Instance.StartBattle();
        }
    }
}


using System.Linq;
using UnityEngine;

namespace Map
{
    public class MapManager :  BaseManager
    {
        [SerializeField] private HexManager grid;
        [SerializeField] private OccupantFactory occupantFactory;
        [SerializeField] private MapData mapData;
        public bool IsTestMode = false;
      
        [ContextMenu("Init New Game")]
        public override void Init()
        {
            grid.GenerateGrid(HexLayout.DataDriven(mapData.Positions));
            foreach (var occupant in mapData.Entries)
            {
                var hex = grid.GetHexAt(occupant.position);
                if (hex == null) continue;
                var instance = occupantFactory.CreateOccupant(occupant.type, grid.transform, hex.transform.position);
                instance?.SetHex(hex);
            }
        }

    }
}


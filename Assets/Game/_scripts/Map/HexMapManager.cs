using UnityEngine;

namespace Map
{
    public class HexMapManager : MonoBehaviour
    {
        [Header("External")]
        public SceneLoader SceneLoader;
        public GameObject BattleScene;
        [Header("Internal")]
        [SerializeField] MapGenerator mapGenerator;
        [SerializeField] PanelController panelController;
        [SerializeField] HexStates mapData;
        public Vector2Int playerPosition { get; private set; } = new Vector2Int(0, 0);
        void Start()
        {
            TilemapSelector.OnTileSelected.AddListener(OnTileSelected);
            mapGenerator.Initialized();

            mapData.hexStates.Clear();
            mapData.hexStates = mapGenerator.CreateMap();
        }
        public HexData GetTile(Vector2Int position)
        {
            return mapData.GetHexState(position).Data;
        }
       
        public void SetPlayerOnTile(Vector2Int position)
        {
            if (!playerPosition.HasRight(position)) return;
            playerPosition = position;
            // update ui
        }
        public Vector3 GetPlayerPostion()
        {
            return mapGenerator.tilemap.GetCellCenterWorld((Vector3Int)playerPosition);
        }
        private void OnTileSelected(Vector2Int arg0)
        {
            SetPlayerOnTile(arg0);
            //SceneLoader.LoadScene(BattleScene);
            //gameObject.SetActive(false);
        }
    }
}

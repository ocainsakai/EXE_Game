using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityServiceLocator;
namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] List<Object> services = new List<Object>();
        private MapGrid mapGrid;
        private MapUI uiManager;
        private GameStates mapState;
        [SerializeField]
        private Player playerPrf;
        [SerializeField]
        private UIPopupManager popupManager;
        private Player player;
        public Vector2Int playerPosition { get; private set; } = new Vector2Int(0, 0);
        private Vector2Int lastPosition = new Vector2Int(0, 0);
        private  Vector2Int startPos = Vector2Int.zero;
        public bool IsTestMode = false;

        private void Awake()
        {
            ServiceLocator sl = ServiceLocator.For(this);
            {
                foreach (var service in services)
                {
                    sl.Register(service.GetType(), service);
                }
            }
           
        }
        private void Start()
        {
            ServiceLocator.For(this)
               .Get(out mapGrid)
               .Get(out mapState)
               .Get(out uiManager);
            HexController.OnHexClicked += OnHexClicked;
            NewGame();
        }
      
        private void NewGame()
        {

            var defaultStates = MapFactory.CreateDefaultMap(mapGrid.mapPosition, mapGrid.mapInitTypes);
            mapGrid.RenderMap(defaultStates);
            mapState.mapStates = mapGrid.mapStates;
            if (player == null)
            {
                CreatePlayer(startPos);
            }
            //popupManager.ShowPopup(UIPopupName.MessageBox);

        }
        private void SaveState()
        {
            mapState.mapStates = mapGrid.mapStates;
            mapState.playerPostion = playerPosition;
            mapState.lastClickPostion = lastPosition;
        }

        public void LoadFromSave()
        {
            
            var loadedStates = MapFactory.CreateFromState(mapState.mapStates.Values.ToList());
            if (loadedStates.Count == 0)
            {
                NewGame();
                return;
            }
            mapGrid.RenderMap(loadedStates);
            mapGrid.mapStates = mapState.mapStates;
            playerPosition = mapState.playerPostion;
            lastPosition = mapState.lastClickPostion;
            if (player == null)
            {
                CreatePlayer(playerPosition);
            }
        }

        private void OnHexClicked(Vector2Int position)
        {
            lastPosition = position;
            var hex = mapGrid.GetState(position);
            bool isValue = playerPosition.HasRight(position);
            uiManager.OpenPopupUI(hex, isValue);
        }

        public async void HandleBattleResult(BattleResult result)
        {
            if (result.IsWin)
            {
                LoadFromSave();
                Debug.Log(playerPosition);
                await UniTask.Delay(1000);  
                await OnExit();
                Debug.Log(playerPosition);

            }
        }

        private async void GoToHexHandle()
        {
            OnEnter();
            if (IsTestMode)
            {
                await OnExit();
                return;
            }
            OnExcutive();
        }

        private void OnExcutive()
        {
            //GameManager.Instance.ChangeScenceToCombat();
        }

        private void OnEnter()
        {
            Debug.Log("on enter");
            var hexes = mapGrid.GetColumnsPos(lastPosition.y);
            Debug.Log(hexes.Count);
            foreach (var hex in hexes)
            {
                if (hex == lastPosition) continue;
                mapGrid.SetNothing(hex);
            }
            SaveState();
        }
        private async UniTask OnExit()
        {
            await ClearColumns();
            SaveState();
            await PlayerMove(lastPosition);
        }

        private async UniTask PlayerMove(Vector2Int position)
        {
            playerPosition = position;
            var worldPosition = mapGrid.tilemap.GetCellCenterWorld((Vector3Int)position);
            await player.transform.DOMove(worldPosition, 0.5f).AsyncWaitForCompletion();
        }

        private async UniTask ClearColumns()
        {
            var hexes = mapGrid.GetColumnsPos(lastPosition.y);
            foreach (var hexpos in hexes)
            {
                if (hexpos == lastPosition)
                {
                    mapGrid.SetNothing(hexpos);
                }
                else
                {
                    mapGrid.SetNone(hexpos);
                }
            }
            await UniTask.Delay(1500);
        }

        private void CreatePlayer(Vector2Int position)
        {
            var worldPosition = mapGrid.tilemap.GetCellCenterWorld((Vector3Int)position);
            player = Instantiate(playerPrf, worldPosition, Quaternion.identity);
        }
    }
}


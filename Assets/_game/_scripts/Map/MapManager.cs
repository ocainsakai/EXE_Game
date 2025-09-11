using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private MapUI mapUI;
        [SerializeField] private GameStates mapState;
        [SerializeField] private Player playerPrf;

        private IGameManager gameManager;
        private ISceneLoader sceneLoader;
        private Player player;
        public Vector2Int playerPosition { get; private set; } = new Vector2Int(0, 0);
        private Vector2Int lastPosition = new Vector2Int(0, 0);
        private  Vector2Int startPos = Vector2Int.zero;
        public bool IsTestMode = false;

        [Inject]
        public void Construct(IGameManager gameManager, ISceneLoader sceneLoader)
        {
            this.gameManager = gameManager; 
            this.sceneLoader = sceneLoader;
        }
        private void Start()
        {
            NewGame();
        }
      
        private void NewGame()
        {

            var defaultStates = MapFactory.CreateDefaultMap(mapGrid.mapPosition, mapGrid.mapInitTypes);
            mapGrid.RenderMap(defaultStates, this);
            mapState.mapStates = mapGrid.mapStates;
            if (player == null)
            {
                CreatePlayer(startPos);
            }

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
            mapGrid.RenderMap(loadedStates, this);
            mapGrid.mapStates = mapState.mapStates;
            playerPosition = mapState.playerPostion;
            lastPosition = mapState.lastClickPostion;
            if (player == null)
            {
                CreatePlayer(playerPosition);
            }
        }

        public void OnHexClicked(Vector2Int position)
        {
            lastPosition = position;
            var hex = mapGrid.GetState(position);
            bool isValue = playerPosition.HasRight(position);
            mapUI.OpenPopupUI(hex, isValue ? ()=> GoToHexHandle() : null);
        }

        //public async void HandleBattleResult(BattleResult result)
        //{
        //    if (result.IsWin)
        //    {
        //        LoadFromSave();
        //        Debug.Log(playerPosition);
        //        await UniTask.Delay(1000);  
        //        await OnExit();
        //        Debug.Log(playerPosition);

        //    }
        //}

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
            sceneLoader.LoadSceneName("Battle").WithData(1);
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


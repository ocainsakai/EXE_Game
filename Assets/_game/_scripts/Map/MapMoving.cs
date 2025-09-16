using Cysharp.Threading.Tasks;
using DG.Tweening;
using Map;
using UnityEngine;

public class MapMoving : MonoBehaviour
{
    [SerializeField] private Transform playerPrf;
    private Transform player;
    public Vector2Int playerPosition { get; private set; } = new Vector2Int(0, 0);
    public Vector2Int lastPosition = new Vector2Int(0, 0);
   
    public void SetPlayerPosition(Vector2Int playerPosition, Vector3 worldPos)
    {
        this.playerPosition = playerPosition;
        if (player == null)
        {
            CreatePlayer(playerPosition, worldPos);
        }
        else
        {
            player.transform.position = worldPos;
        }

    }
    public bool OnHexClicked(Vector2Int position)
    {
        lastPosition = position;
        return playerPosition.HasRight(position);
    }
    private void OnEnter(MapGrid mapGrid)
    {
        Debug.Log("on enter");
        var hexes = mapGrid.GetColumnsPos(lastPosition.y);
        Debug.Log(hexes.Count);
        foreach (var hex in hexes)
        {
            if (hex == lastPosition) continue;
            mapGrid.SetNothing(hex);
        }
    }
    private async UniTask OnExit(MapGrid mapGrid)
    {
        await ClearColumns(mapGrid);
        //SaveState();
        await PlayerMove(lastPosition, mapGrid);
    }

    private async UniTask ClearColumns(MapGrid mapGrid)
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

    private async UniTask PlayerMove(Vector2Int position, MapGrid mapGrid)
    {
        playerPosition = position;
        var worldPosition = mapGrid.tilemap.GetCellCenterWorld((Vector3Int)position);
        await player.transform.DOMove(worldPosition, 0.5f).AsyncWaitForCompletion();
    }
    private void CreatePlayer(Vector2Int position, Vector3 worldPos)
    {
        player = Instantiate(playerPrf, worldPos, Quaternion.identity);
    }

    public void MoveTo(Vector2Int position, MapGrid mapGrid)
    {
        OnEnter(mapGrid);
        RunManager.Instance.Battle();
    }
}

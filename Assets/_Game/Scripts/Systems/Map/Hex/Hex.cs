using UnityEngine;

public class Hex : MonoBehaviour, IHex<Hex>
{
    [SerializeField] private Vector2Int hexPosition;
    public Vector2Int HexPosition => hexPosition;
    protected HexManager manager;
    public Hex GetNeighbor(HexDirection direction)
    {
        var offset = direction.GetOffset(HexPosition.y);
        var neighborPos = HexPosition + offset;
        return manager.GetHexAt(neighborPos);
    }

    public Hex GetNeighbor(int direction)
    {
        return GetNeighbor((HexDirection)direction);
    }

    public void SetHexPosition(Vector2Int position)
    {
        hexPosition = position;
    }

    public void Initialize(Vector2Int pos, HexManager hexManager)
    {
        hexPosition = pos;
        manager = hexManager;
    }

    
}

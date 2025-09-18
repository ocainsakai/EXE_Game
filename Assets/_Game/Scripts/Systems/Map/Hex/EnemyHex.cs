using Map;
using UnityEngine;

public class EnemyHex : MonoBehaviour, IHexTileSelector<Hex>, IHexOccupant<Hex>
{
    private Hex currentHex;
    public Hex CurrentHex => currentHex;

    [SerializeField]
    private EnemyData enemyData;
    public EnemyData EnemyData => enemyData;
    public void SetHex(Hex hex)
    {
        if (currentHex != null)
        {
            currentHex.transform.DetachChildren();
        }
        currentHex = hex;
        transform.SetParent(hex.transform, false);
        transform.localPosition = Vector3.zero;
    }
    public void DeselectTile(IHexOccupant<Hex> tile)
    {
    }

    public void SelectTile(IHexOccupant<Hex> tile)
    {
        Debug.Log($"Selected Enemy Hex at {CurrentHex.HexPosition} with Enemy: {EnemyData.name}");
    }

    public void OnEnter()
    {
    }

    public void OnLeave()
    {
    }

    private void OnMouseDown()
    {
        if (MapPopup.IsShowing) return;  // neu dang hien popup thi khong lam gi ca
        SelectTile(this);
        UIManager.Instance.GetType<MapPopup>().Show(enemyData);
    }
}

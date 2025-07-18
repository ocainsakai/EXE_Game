using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class HexData : MonoBehaviour
{
    // Data cho từng ô
    public int row;
    public int col;
    public string tileType;
    public bool isOccupied;

    // Hàm khởi tạo data
    public void Initialize(int row, int col, string type = "Normal")
    {
        this.row = row;
        this.col = col;
        this.tileType = type;
        this.isOccupied = false;
    }

    private void OnMouseDown()
    {
        Debug.Log($"HexData clicked: ({row}, {col}), Type: {tileType}");
        TileInfoPanelController.Instance.Show(tileType);
    }
}

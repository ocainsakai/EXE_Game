using UnityEngine;
using UnityEngine.Events;

public class UIBattle : MonoBehaviour
{
    public void Show(EnemyData enemy)
    {
        Debug.Log($"[UIBattle] Show battle UI for enemy: {enemy.Name}");
        // Hiển thị thông tin kẻ thù trên giao diện người dùng
    }
}

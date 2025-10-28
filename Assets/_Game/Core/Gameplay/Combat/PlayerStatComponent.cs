using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerStatComponent : MonoBehaviour, IHealth
{
    [SerializeField] private UISliderBarHelper healthBarHelper;
    [SerializeField] private PlayerData playerData; // SO chứa base stats

    // --- DỮ LIỆU RUNTIME (Trạng thái hiện tại) ---
    public Health health;
    public int Gold { get; private set; }

    // --- KHỞI TẠO ---
    private void Start()
    {
        InitializeStats();
    }

    /// <summary>
    /// Quyết định xem nên tải game (Load) hay bắt đầu game mới (New Game)
    /// </summary>
    private void InitializeStats()
    {
        // 1. Lấy chỉ số Max HP từ ScriptableObject
        int maxHp = (int)playerData.hp;

        // 2. Kiểm tra xem có save file không
        /*if (PlayerSave.HasSave()) // (Bạn cần tự tạo hàm HasSave() trong PlayerSave)
        {
            // --- TRƯỜNG HỢP LOAD GAME ---
            health = new Health(currentHp, maxHp); // Máu hiện tại = đã lưu, Max = từ SO

            Gold = PlayerSave.GetPlayerCoin(); // Tải vàng đã lưu
        }
        else*/
        {
            // --- TRƯỜNG HỢP NEW GAME ---
            health = new Health(maxHp, maxHp); // Máu đầy
            Gold = PlayerSave.GetPlayerCoin(); // Vàng khởi điểm từ SO
            
            // (Bạn có thể save lại trạng thái khởi điểm ở đây nếu muốn)
            /*
            PlayerSave.SetPlayerHp(maxHp);
            */
            PlayerSave.SetPlayerCoin(Gold);
        }
        
        // 3. Kết nối UI và sự kiện Auto-Save
        health.onValueChanged.AddListener(healthBarHelper.SetValue);
        /*
        health.onValueChanged.AddListener(OnHealthChanged); // Tự động save khi máu thay đổi
    */
    }

    // --- API QUẢN LÝ VÀNG (GOLD) ---

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        Gold += amount;
        PlayerSave.SetPlayerCoin(Gold); // Lưu lại ngay
        
        // (Nếu có UI cho Gold, cập nhật ở đây)
        // UIManager.Instance.UpdateGoldText(Gold); 
    }

    public bool SpendGold(int amount)
    {
        if (amount <= 0) return false;
        
        if (Gold >= amount)
        {
            Gold -= amount;
            PlayerSave.SetPlayerCoin(Gold); // Lưu lại ngay
            // UIManager.Instance.UpdateGoldText(Gold);
            return true; // Chi tiêu thành công
        }
        else
        {
            return false; // Không đủ tiền
        }
    }

    // --- API QUẢN LÝ MÁU (HEALTH) ---

    public void TakeDame(float damage)
    {
        health.Subtract((int)damage);
        // Việc lưu HP đã được xử lý tự động bởi OnHealthChanged
    }

    /// <summary>
    /// Được gọi tự động mỗi khi máu thay đổi (thông qua onValueChanged).
    /// </summary>
    /*private void OnHealthChanged(float current, float max)
    {
        // Tự động lưu lại HP
        PlayerSave.SetPlayerHp((int)current); 
    }*/

    // --- DEBUG ---
#if UNITY_EDITOR
    [ContextMenu("Test Take 10 Dame")]
    private void TestTakeDame()
    {
        TakeDame(10f);
    }
    
    [ContextMenu("Test Add 100 Gold")]
    private void TestAddGold()
    {
        AddGold(100);
    }
#endif
}

public interface IHealth
{
    public void TakeDame(float damage);
}

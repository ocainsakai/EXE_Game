using UnityEngine;
using UnityEngine.UI;

public abstract class BaseScreen : MonoBehaviour
{
    [Header("Base Buttons")]
    // Dùng 'protected' để class con có thể thấy
    [SerializeField] protected Button button1; // Nút "Claim"
    [SerializeField] protected Button button2; // Nút "Claim x2 (Ad)"
    [SerializeField] protected Button button3; // Nút "Close" (nếu có)

    // Dùng 'virtual' để class con có thể 'override' nếu cần
    protected virtual void Awake()
    {
        // Tự động đăng ký các hàm listener
        // Class con sẽ định nghĩa hàm OnButton1Clicked...
        if (button1 != null)
            button1.onClick.AddListener(OnButton1Clicked); 
        
        if (button2 != null)
            button2.onClick.AddListener(OnButton2Clicked);
        
        if (button3 != null)
            button3.onClick.AddListener(OnButton3Clicked);
            
        // Bắt đầu ở trạng thái ẩn
        gameObject.SetActive(false);
    }

    // Các hàm này được thiết kế để class con "ghi đè" (override)
    protected abstract void OnButton1Clicked();
    protected abstract void OnButton2Clicked();
    
    // Nút 3 (Close) thường có hành vi chung là tự đóng nó lại
    protected virtual void OnButton3Clicked()
    {
        Hide();
    }

    // Hành vi chung
    public virtual void Show()
    {
        gameObject.SetActive(true);
        // Có thể thêm logic animation vào đây
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        // Có thể thêm logic animation vào đây
    }
    
    // Đảm bảo hủy đăng ký listener khi object bị hủy
    protected virtual void OnDestroy()
    {
        if (button1 != null) button1.onClick.RemoveListener(OnButton1Clicked);
        if (button2 != null) button2.onClick.RemoveListener(OnButton2Clicked);
        if (button3 != null) button3.onClick.RemoveListener(OnButton3Clicked);
    }
}

// Kế thừa từ BaseScreen
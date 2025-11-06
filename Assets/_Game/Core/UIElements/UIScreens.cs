using _Game.Core.Gameplay;
using BulletHellTemplate;
using UnityEngine;

namespace _Game.Core.UIElements
{
    // Class này giờ chỉ làm 1 việc: QUẢN LÝ (Orchestrator)
    public class UIScreens : MonoBehaviour
    {
        // Sửa kiểu dữ liệu từ UIScreenElement thành BaseScreen
        // Kéo các Prefab/GameObject (đã gắn UIWinScreen, UILoseScreen...) vào đây
        [SerializeField] private BaseScreen winScreens; 
        [SerializeField] private BaseScreen completeScreens;
        [SerializeField] private BaseScreen loseScreens;
        [SerializeField] private BaseScreen pauseScreens;

        [SerializeField] private AudioClip winVfx;
        [SerializeField] private AudioClip loseVfx;

        // XÓA TOÀN BỘ HÀM AWAKE()
        // Hàm Awake() bây giờ không còn gì cả, 
        // vì mỗi screen đã tự đăng ký listener

        public void CloseAllScreens()
        {
            // Gọi hàm Hide() thay vì SetActive(false)
            winScreens.Hide();
            completeScreens.Hide();
            loseScreens.Hide();
            pauseScreens.Hide();
        }
        
        public void ShowWinScreens()
        {
            CloseAllScreens();
            AudioManager.Singleton.PlayAudio(winVfx, "master");
            winScreens.gameObject.SetActive(true);
            winScreens.Show(); // Gọi hàm Show()
        }
        
        public void ShowLoseScreens()
        {
            CloseAllScreens();
            AudioManager.Singleton.PlayAudio(loseVfx, "master");
            loseScreens.gameObject.SetActive(true);
            loseScreens.Show(); // Gọi hàm Show()
        }

        public void ShowCompleteScreens()
        {
            CloseAllScreens();
            AudioManager.Singleton.PlayAudio(winVfx, "master");
            completeScreens.gameObject.SetActive(true);
            completeScreens.Show(); // Gọi hàm Show()
        }
        
        public void ShowPauseScreens()
        {
            CloseAllScreens();
            
            pauseScreens.Show(); // Gọi hàm Show()
        }
    }
}
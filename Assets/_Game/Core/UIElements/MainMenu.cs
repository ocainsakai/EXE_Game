using UnityEngine;
using UnityEngine.UI; // <-- Thêm dòng này
using _Game.Core;
using _Game.Core.Gameplay;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;


    private void Start()
    {
        // Tự động kiểm tra xem file save có tồn tại không
        bool saveFileExists = PlayerPrefs.HasKey(MapManager.SAVE_KEY);

        // Nếu file save tồn tại, cho phép bấm
        // Nếu không, vô hiệu hóa (làm mờ) nút
        continueButton.interactable = saveFileExists;
    }
    public void OnContinueClicked()
    {
        // Chỉ cần tải scene.
        // MapManager.Start() sẽ tự động gọi LoadMap().
        SceneLoader.Instance.LoadScene("Map");
    }
}

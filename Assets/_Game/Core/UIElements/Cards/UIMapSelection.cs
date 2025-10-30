using System;
using _Game.Core;
using _Game.Core.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class UIMapSelection : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Kéo GameObject/Prefab có script MapEntry vào đây")]
    [SerializeField] private MapEntry mapView;

    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;

    [Tooltip("Hình ảnh mặc định nếu MapData không cung cấp hình nền")]
    [SerializeField] private Sprite defaultMapBackground;

    [SerializeField] private Button selectBtn;

    private int currentMapIndex;

    private MapData CurrentDetailMap
    {
        get
        {
            // Kiểm tra xem GameInstance và mảng maps có tồn tại không
            if (GameInstance.Singleton == null || GameInstance.Singleton.maps == null || GameInstance.Singleton.maps.Length == 0)
                return null;
            // Kiểm tra xem chỉ số có hợp lệ không
            if (currentMapIndex < 0 || currentMapIndex >= GameInstance.Singleton.maps.Length)
                return null;

            return GameInstance.Singleton.maps[currentMapIndex];
        }
    }

    // Lấy tổng số lượng map từ mảng 'maps'
    private int Max => (GameInstance.Singleton != null && GameInstance.Singleton.maps != null) ? GameInstance.Singleton.maps.Length : 0;

    private void OnEnable()
    {
        leftBtn.onClick.AddListener(OnLeftBtnClicked);
        rightBtn.onClick.AddListener(OnRightBtnClicked);
        selectBtn.onClick.AddListener(OnSelectBtnClicked);

        currentMapIndex = PlayerSave.GetSelectedMap();

        // Kiểm tra nếu chỉ số lưu bị lỗi
        if (Max == 0 || currentMapIndex < 0 || currentMapIndex >= Max)
        {
            currentMapIndex = 0; // Quay về map đầu tiên
        }

        UpdateMapView();
    }

    private void OnDisable()
    {
        leftBtn.onClick.RemoveListener(OnLeftBtnClicked);
        rightBtn.onClick.RemoveListener(OnRightBtnClicked);
        selectBtn.onClick.RemoveListener(OnSelectBtnClicked);
    }

    private void OnSelectBtnClicked()
    {
        var selectedMap = CurrentDetailMap;

        // --- Cải tiến: Thêm kiểm tra an toàn ---
        if (selectedMap == null)
        {
            Debug.LogError("Lỗi: Không thể chọn map vì CurrentDetailMap là null.");
            return;
        }

        if (GameInstance.Singleton == null)
        {
            Debug.LogError("Lỗi: GameInstance.Singleton là null.");
            return;
        }

        if (SceneLoader.Instance == null)
        {
            Debug.LogError("Lỗi: SceneLoader.Instance là null.");
            return;
        }

        // 1. Lưu map nào VỪA ĐƯỢC CHỌN
        PlayerSave.SetSelectedMap(currentMapIndex);
        GameInstance.Singleton.SetCurrentMap(selectedMap.mapID);

        // 2. --- THAY ĐỔI CHÍNH ---
        // Xóa bất kỳ tiến trình cũ nào đã lưu của map.
        // Điều này sẽ buộc MapManager.Start() gọi CreateNewMap().
        MapManager.DeleteMapSave();

        // 3. Tải scene "Map"
        SceneLoader.Instance.LoadScene("Map");
        Debug.Log($"Đang TẠO MAP MỚI: {selectedMap.mapName} (ID: {selectedMap.mapID}).");
    }

    private void OnRightBtnClicked()
    {
        if (Max == 0) return; 
        currentMapIndex++;
        if (currentMapIndex >= Max)
            currentMapIndex = 0; 
        UpdateMapView();
    }

    private void OnLeftBtnClicked()
    {
        if (Max == 0) return; 
        currentMapIndex--;
        if (currentMapIndex < 0)
            currentMapIndex = Max - 1; 
        UpdateMapView();
    }

    private void UpdateMapView()
    {
        if (mapView == null) return;

        var map = CurrentDetailMap;

        if (map != null)
        {
            mapView.gameObject.SetActive(true);
            mapView.SetMapName(map.mapName);

            // Sử dụng trường 'mapBackground' từ MapData
            if (map.mapBackground != null)
            {
                mapView.SetMapBackground(map.mapBackground);
            }
            else
            {
                mapView.SetMapBackground(defaultMapBackground);
            }
        }
        else
        {
            // Trường hợp không có map nào trong GameInstance
            mapView.gameObject.SetActive(true);
            mapView.SetMapName("No Map Available");
            mapView.SetMapBackground(defaultMapBackground);
        }
    }
}
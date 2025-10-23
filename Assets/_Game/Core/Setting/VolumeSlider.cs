using BulletHellTemplate;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType
    {
        master,
        vfx,
        ambience,
    }
    public VolumeType volumeType;
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        
        // 1. Gán giá trị hiện tại của AudioManager cho Slider
        switch (volumeType)
        {
            case VolumeType.master:
                slider.value = AudioManager.Singleton.masterVolume;
                break;
            case VolumeType.vfx:
                slider.value = AudioManager.Singleton.vfxVolume;
                break;
            case VolumeType.ambience:
                slider.value = AudioManager.Singleton.ambienceVolume;
                break;
        }
        
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        // 2. Thêm listener để khi kéo Slider, nó gọi hàm của AudioManager
    }

    public void OnSliderValueChanged(float value)
    {
        switch (volumeType)
        {
            case VolumeType.master:
                AudioManager.Singleton.SetMasterVolume(value);
                break;
            case VolumeType.vfx:
                AudioManager.Singleton.SetVFXVolume(value);
                break;
            case VolumeType.ambience:
                AudioManager.Singleton.SetAmbienceVolume(value);
                break;
        }
        // Cập nhật âm lượng Master
    }
}
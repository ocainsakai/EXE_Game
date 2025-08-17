using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Slider _colorSlider;
    [SerializeField] private Image _playerRenderer;


    [SerializeField] PlayerBehaviour _playerBehaviour;
    private void Start()
    {
        _colorSlider.onValueChanged.AddListener(UpdatePlayerColor);
        UpdatePlayerColor(_colorSlider.value); 
    }

    private void UpdatePlayerColor(float value)
    {
        Color newColor = Color.HSVToRGB(value, 1f, 1f);
        _playerRenderer.color = newColor;
    }

    public void OnRegisterClicked()
    {
        string playerName = _playerNameInput.text;
        Color colorValue = _playerRenderer.color;

        _playerBehaviour.UpdatePlayerData(new PlayerData(playerName, colorValue));

        // Convert sang JSON

    }
}

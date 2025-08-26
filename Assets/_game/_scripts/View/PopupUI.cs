using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class PopupUI : MonoBehaviour
    {
        public static Action onClick;
        [SerializeField] TextMeshProUGUI hexInfoText;
        [SerializeField] Button enterButton;
        public void UpdateUI(HexRuntime data, bool isValue)
        {
            hexInfoText.text = $"Hex Type: {data.Type}\nPosition: {data.position}";
            enterButton.gameObject.SetActive(isValue);
            enterButton.onClick.RemoveAllListeners();
            enterButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
            });
        }
    }
}
using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Image artImage;

    public void SetCard(Sprite data)
    {
        if (artImage != null) artImage.sprite = data;
    }
}

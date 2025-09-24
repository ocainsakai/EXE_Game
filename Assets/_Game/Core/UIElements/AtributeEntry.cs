using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtributeEntry : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _count;

    public void SetData(Sprite icon, int count)
    {
        _icon.sprite = icon;
        _count.text = count.ToString();
    }
}

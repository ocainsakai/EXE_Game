using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AtributeEntry : MonoBehaviour
{
    [FormerlySerializedAs("_icon")] [SerializeField] Image icon;
    [FormerlySerializedAs("_count")] [SerializeField] TextMeshProUGUI count;

    public void SetData(Sprite icon, int count)
    {
        this.icon.sprite = icon;
        this.count.text = count.ToString();
    }
}

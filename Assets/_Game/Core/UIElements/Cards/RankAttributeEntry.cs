using TMPro;
using UnityEngine;

public class RankAttributeEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attributeNameText;
    [SerializeField] private TextMeshProUGUI attributeCountText;

    public void SetAttribute(string attributeName, int attributeCount)
    {
        attributeNameText.text = attributeName;
        attributeCountText.text = attributeCount.ToString();
    }
}

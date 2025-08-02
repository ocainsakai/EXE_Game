using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectionButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    private EnemyData _enemyData;
    private System.Action<EnemyData> _onClick;

    public void Initialize(EnemyData data, System.Action<EnemyData> onClick)
    {
        _enemyData = data;
        _onClick = onClick;
        _icon.sprite = data.Icon;
        _nameText.text = data.DisplayName;
    }

    public void OnButtonClick() => _onClick?.Invoke(_enemyData);
}

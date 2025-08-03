using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectionButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Button _button;
    //private Action<EnemyData> _onClick;

    public void Initialize(EnemyData data, Action<EnemyData> onClick)
    {
        _enemyData = data;
        //_onClick = onClick;
        _icon.sprite = data.Icon;
        _nameText.text = data.DisplayName;

        _button?.onClick.AddListener(() => onClick?.Invoke(_enemyData));
    }
}

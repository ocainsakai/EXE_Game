using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectionButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Enemy _enemyData;
    [SerializeField] private Button _button;
    //private Action<EnemyData> _onClick;

    public void Initialize(Enemy enemy, Action<Enemy> onClick)
    {
        //_nameText.text = enemy.;        
        _enemyData = enemy;

        _icon.sprite = enemy.Data.Icon;

        _coinText.text = $"Cost: {enemy.Data.cost}";
        _rewardText.text = $"Reward: {enemy.Data.reward}";

        _button?.onClick.AddListener(() => onClick?.Invoke(_enemyData));
    }
}

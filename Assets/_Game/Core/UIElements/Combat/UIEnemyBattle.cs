using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEnemyBattle : MonoBehaviour
{
    [SerializeField] Enemy currentEnemy;
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI enemyNameText;
    [SerializeField] UISliderBarHelper health;
    [SerializeField] UISliderBarHelper action;

    public void SetEnemy(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.count.onValueChanged.AddListener((current, max) =>
        {
            action.SetValue(current, max);
        });
        currentEnemy.health.onValueChanged.AddListener((current, max) =>
        {
            health.SetValue(current, max);
        });
        health.SetValue(currentEnemy.health.CurrentValue, currentEnemy.health.MaxValue);
        action.SetValue(currentEnemy.count.CurrentValue, currentEnemy.count.MaxValue);
        Show(currentEnemy);
    }

    public void Show(Enemy enemy)
    {
        avatar.sprite = enemy.data.icon;
        enemyNameText.text = enemy.data.nameDisplay;
    }

    public void OnTakeDameHandler(Enemy enemy)
    {
        Show(enemy);
    }
}

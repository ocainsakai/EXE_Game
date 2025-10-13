using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEnemyBattle : MonoBehaviour
{
    [SerializeField] Enemy currentEnemy;
    
    
    [SerializeField] Image avatar;
    [SerializeField] Slider healthBar;
    [SerializeField] Slider actionBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI enemyNameText;

    public void Show(Enemy enemy)
    {
        avatar.sprite = enemy.Data.Icon;
        healthBar.maxValue = enemy.MaxHP;  
        healthBar.value = enemy.HP;
        healthText.text = $"{enemy.HP}/{enemy.MaxHP}";
        actionBar.maxValue = enemy.MaxActionCount;
        actionBar.value = enemy.CurrentActionCount;

        enemyNameText.text = enemy.Data.Name;
    }
    public void OnTakeDameHandler(Enemy enemy)
    {
        Show(enemy);
    }
}

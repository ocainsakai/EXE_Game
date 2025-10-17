using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEnemyBattle : MonoBehaviour
{
    [SerializeField] Enemy currentEnemy;
    
    
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI enemyNameText;


    public void Show(Enemy enemy)
    {
        avatar.sprite = enemy.Data.Icon;
        enemyNameText.text = enemy.Data.Name;
    }

    public void OnTakeDameHandler(Enemy enemy)
    {
        Show(enemy);
    }
}

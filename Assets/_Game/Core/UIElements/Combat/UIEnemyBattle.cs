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
        avatar.sprite = enemy.data.icon;
        enemyNameText.text = enemy.data.name;
    }

    public void OnTakeDameHandler(Enemy enemy)
    {
        Show(enemy);
    }
}

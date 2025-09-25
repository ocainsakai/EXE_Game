using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBattle : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI enemyNameText;
    public void Show(EnemyData enemy)
    {
        Debug.Log($"[UIBattle] Show battle UI for enemy: {enemy.Name}");
        avatar.sprite = enemy.Icon;
        healthBar.maxValue = enemy.HP;  
        healthBar.value = enemy.HP;
        healthText.text = $"{enemy.HP}/{enemy.HP}";
        enemyNameText.text = enemy.Name;
    }

}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private HealthDisplay enemyHealth;
    [SerializeField] private BattleEnemy enemyManager;
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI NameText;
    private void OnEnable()
    {
        enemyManager.onEnemyChanged += UpdateUI;
        enemyHealth.gameObject.SetActive(true);
    }

  

    private void OnDisable()
    {
        enemyManager.onEnemyChanged -= UpdateUI;
        enemyHealth.gameObject.SetActive(false);
    }

    private void UpdateUI(Enemy enemy)
    {
        if (enemy != null)
        {
            //Image.sprite = enemy.Data.Icon;
            //NameText.text = enemy.Data.DisplayName;
        }
    }
}

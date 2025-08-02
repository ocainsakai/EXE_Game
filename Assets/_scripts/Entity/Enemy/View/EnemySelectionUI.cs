using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelectionUI : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _enemyDB;
    [SerializeField] private Transform _enemyButtonContainer;
    [SerializeField] private GameObject _enemyButtonPrefab;

    private void Start()
    {
        foreach (var enemy in _enemyDB.Enemies)
        {
            var button = Instantiate(_enemyButtonPrefab, _enemyButtonContainer);
            button.GetComponent<EnemySelectionButton>().Initialize(enemy, OnEnemySelected);
        }
    }

    private void OnEnemySelected(EnemyData selectedEnemy)
    {
        CombatSimulationManager.Instance.SetSelectedEnemy(selectedEnemy);
        SceneManager.LoadScene("BattleScene");
    }
}

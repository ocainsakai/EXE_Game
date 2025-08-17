using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySelectionUI : MonoBehaviour
{
    [SerializeField] private Transform _enemyButtonContainer;
    [SerializeField] private GameObject _enemyButtonPrefab;
    [SerializeField] private EnemyDatabase _enemyDB;
    [SerializeField] private EnemySelectedUI _enemySelectedUI;
    [SerializeField] private CoinRSO _playerCoinRSO;
    private void Start()
    {
        foreach (var enemy in _enemyDB.Enemies)
        {
            var button = Instantiate(_enemyButtonPrefab, _enemyButtonContainer);
            button.GetComponent<EnemySelectionButton>().Initialize(enemy, OnEnemySelected);
        }
    }

    private void OnEnemySelected(Enemy selectedEnemy)
    {
        if (_playerCoinRSO.onwnerCoins.Value < selectedEnemy.Data.cost)
        {
            Debug.LogWarning("Not enough coins to select this enemy.");
            return;
        }
        _playerCoinRSO.onwnerCoins.Value -= selectedEnemy.Data.cost;
        _enemySelectedUI.AddEnemySelected(selectedEnemy);
    }
}

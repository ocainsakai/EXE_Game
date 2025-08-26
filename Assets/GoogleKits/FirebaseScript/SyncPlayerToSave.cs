using UnityEngine;

public class SyncPlayerToSave : MonoBehaviour
{
    [SerializeField] PlayerSaveManager _playerSaveManager;
    [SerializeField] PlayerBehaviour _playerBehaviour;

    private void Reset()
    {
        _playerSaveManager = FindFirstObjectByType<PlayerSaveManager>();

    }

    private void Start()
    {
       
        _playerSaveManager.OnPlayerUpdateEvent.AddListener(HandlePlayerSaveUpdate);
        _playerBehaviour.OnPlayerDataChanged.AddListener(SavePlayerData);
        _playerBehaviour.UpdatePlayerData(_playerSaveManager.LastPlayerData);
    }
    private void HandlePlayerSaveUpdate(PlayerData playerData)
    {
        _playerBehaviour.UpdatePlayerData(playerData);
    }
    private void SavePlayerData()
    {
        _playerSaveManager.SavePlayer(_playerBehaviour.PlayerData);
    }
}

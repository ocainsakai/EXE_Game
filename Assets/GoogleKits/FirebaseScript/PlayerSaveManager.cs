using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;


public class PlayerSaveManager : MonoBehaviour
{
    private const string PLAYER_KEY = "PLAYER_KEY";
    public PlayerData LastPlayerData { get; private set; }
    public UnityEvent<PlayerData> OnPlayerUpdateEvent = new UnityEvent<PlayerData>();
    private FirebaseDatabase _database;
    private DatabaseReference _ref;
    private void Awake()
    {
        _database = FirebaseDatabase.DefaultInstance;
        _ref = _database.GetReference(PLAYER_KEY);
        _ref.ValueChanged += OnPlayerDataChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the ValueChanged event if needed
        _ref.ValueChanged -= OnPlayerDataChanged;
        _ref = null;
        _database = null;
    }

    private void OnPlayerDataChanged(object sender, ValueChangedEventArgs e)
    {
        var json = e.Snapshot.GetRawJsonValue();

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("Player data is empty or null.");
            return;
        }
        var playerData = JsonUtility.FromJson<PlayerData>(json);
        LastPlayerData = playerData;
        OnPlayerUpdateEvent?.Invoke(playerData);
        Debug.Log($"Player data loaded: {playerData}");

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SavePlayer(PlayerData playerData)
    {
        if (playerData.Equals(LastPlayerData))
        {
            Debug.Log("Player data is unchanged, skipping save.");
            return;
        }
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_KEY, json);
        PlayerPrefs.Save();
        _database.GetReference(PLAYER_KEY).SetRawJsonValueAsync(json);
        Debug.Log("Player data saved successfully.");
    }

    public async UniTask<PlayerData> LoadPlayer()
    {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists)
        {
            return null;
        }
        
        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async UniTask<bool> SaveExists()
    {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        return dataSnapshot.Exists && !string.IsNullOrEmpty(dataSnapshot.GetRawJsonValue());
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(PLAYER_KEY);
        _database.GetReference(PLAYER_KEY).RemoveValueAsync();
        Debug.Log("Player save deleted successfully.");
    }
}

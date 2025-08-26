using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    //[SerializeField] PlayerSaveManager _playerSaveManager;
    [SerializeField] TMP_Text _playerNameText;
    [SerializeField]
    private PlayerData _playerData;
    //public PlayerUpdateEvent OnPlayerUpdateEvent;
    public PlayerData PlayerData
    {
        get { return _playerData; }
        set { _playerData = value; }
    }

    public string PlayerName
    {
        get { return _playerData.playerName; }
        set { _playerData.playerName = value; }
    }

    public Color PlayerColor
    {
        get { return _playerData.playerColor; }
        set { _playerData.playerColor = value; }
    }

    public int PlayerScore
    {
        get { return _playerData.playerScore; }
        set { _playerData.playerScore = value; }
    }

    public UnityEvent OnPlayerDataChanged = new();

    public void AddScore(int score)
    {
        if (score > 0)
        {
            PlayerScore += score;
            OnPlayerDataChanged.Invoke();
        }
    }
    
    public void UpdatePlayerData(PlayerData newData)
    {
        if (newData != null)
        {
            PlayerData = newData;
            _playerNameText.text = PlayerName;
            GetComponent<SpriteRenderer>().color = PlayerColor;
            OnPlayerDataChanged.Invoke();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddScore(10);
        }
    }
}

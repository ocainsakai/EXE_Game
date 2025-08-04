using Ain;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
public class PlayerController : Singleton<PlayerController>
{
    [Header("Dependencies")]
    [SerializeField] private Button[] actionButtons;

    [SerializeField] private HealthComponent _healthComponent;
    private Health _health;
    private PlayerStateMachine _sm;
    public PlayerHandController handController;

    [SerializeField] public CardList deckList;
    [SerializeField] public CoinRSO coinRSO;
    // Private Fields
    private bool _isInitialized = false;

    // Properties
    public Health PlayerHealth => _health;
    public PlayerHandController HandController => handController;
    // action
    public Action OnPlayerEndTurn;
    protected override void Awake()
    {
        base.Awake();
        InitializeComponents();
        SetupActionButtons();
        _sm = new PlayerStateMachine(this);
        _sm.ChangeState(PlayerStateType.Idle);
    }

    private void InitializeComponents()
    {
        _health = new Health(_healthComponent);
        _health.OnDamageTaken.Subscribe(x => Debug.Log($"Player took damage: {x}")).AddTo(this);
        _health.OnDeath.Subscribe(async x => 
        {
            Debug.Log("Player has died.");
            await UniTask.Delay(1000);
            BattleManager.Instance.LoseBattle();
        }).AddTo(this);
        _healthComponent.Initialize(100, 0);
    }
    public void Initialize()
    {
        if (_isInitialized) return;
        handController.Initialize(this);
        handController.DrawCard(deckList);
        _isInitialized = true;
    }
    private void OnDestroy()
    {
        // Clean up event listeners
        foreach (var button in actionButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }


    // Button click handlers
    #region Button Click Handlers
    private void SetupActionButtons()
    {
        if (actionButtons == null || actionButtons.Length < 4)
        {
            Debug.LogError("Action buttons not properly assigned!");
            return;
        }

        // Set button texts
        actionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
        actionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "DISCARD";
        actionButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "SORT";
        actionButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = "SKIP";

        // Clear existing listeners to prevent duplicates
        foreach (var button in actionButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        // Add new listeners
        actionButtons[0].onClick.AddListener(OnPlayButtonClicked);
        actionButtons[1].onClick.AddListener(OnDiscardButtonClicked);
        actionButtons[2].onClick.AddListener(OnSortButtonClicked);
        // actionButtons[3] left for future use

        DisableAllActions();
    }
    private void OnPlayButtonClicked()
    {
        if (!_isInitialized) return;
        Attack();
    }

    private void OnDiscardButtonClicked()
    {
        if (!_isInitialized) return;
        handController.Discard(handController.Hand);
    }

    private void OnSortButtonClicked()
    {
        if (!_isInitialized) return;
        handController.Sort();
    }

    #endregion
    // Action control methods
    #region Action Control Methods
    public void EnableAllActions()
    {
        foreach (var button in actionButtons)
        {
            button.interactable = true;
        }
    }

    public void DisableAllActions()
    {
        foreach (var button in actionButtons)
        {
            button.interactable = false;
        }
    }
    #endregion
    // Turn management
    #region Turn Management
    public void StartTurn()
    {
        _sm.ChangeState(PlayerStateType.Action);
    }

    public void Attack()
    {
        if (handController.Hand.cards.Count == 0)
        {
            Debug.LogWarning("No cards selected for attack.");
            return;
        }
        Debug.Log("Attack Phase Started");
        _sm.ChangeState(PlayerStateType.Attack);
    }

    public void EndTurn()
    {
        Debug.Log("End Turn");
        DisableAllActions();
        OnPlayerEndTurn?.Invoke();
    }

    internal bool IsPlayerDead()
    {
        return _healthComponent.CurrentHP.Value <= 0;
    }
    #endregion
}
using CardSystem.PokerSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controller cho UI battle screen với Energy System
/// </summary>
public class UIBattleController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleSystem battleSystem;

    [Header("HP Display")]
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private Slider playerHPSlider;
    [SerializeField] private Slider enemyHPSlider;

    [Header("Energy Display")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Slider energySlider;
    [SerializeField] private TextMeshProUGUI roundText;

    [Header("Enemy Info")]
    [SerializeField] private Image enemyIcon;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Battle Log")]
    [SerializeField] private TextMeshProUGUI battleLogText;
    [SerializeField] private int maxLogLines = 10;

    [Header("Buttons")]
    [SerializeField] private Button playHandButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private Button endTurnButton;

    [Header("Hand Selection")]
    [SerializeField] private UIHandSelector handSelector;

    [Header("Battle End")]
    [SerializeField] private GameObject battleEndPanel;
    [SerializeField] private TextMeshProUGUI battleResultText;
    [SerializeField] private Button continueButton;

    private void OnEnable()
    {
        SubscribeEvents();
        SetupButtons();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
        CleanupButtons();
    }

    // ==================== EVENT SUBSCRIPTION ====================

    private void SubscribeEvents()
    {
        var events = battleSystem.Events;

        events.OnBattleStart.AddListener(OnBattleStart);
        events.OnStateChanged.AddListener(UpdateUI);
        events.OnHandPlayed.AddListener(OnHandPlayed);
        events.OnPlayerDamaged.AddListener(OnPlayerDamaged);
        events.OnEnemyDamaged.AddListener(OnEnemyDamaged);
        events.OnEnemyTurn.AddListener(OnEnemyTurn);
        events.OnRoundStart.AddListener(OnRoundStart);
        events.OnBattleEnd.AddListener(OnBattleEnd);
        events.OnEnergyChanged.AddListener(OnEnergyChanged);
        events.OnEnergyDepleted.AddListener(OnEnergyDepleted);
    }

    private void UnsubscribeEvents()
    {
        var events = battleSystem.Events;

        events.OnBattleStart.RemoveListener(OnBattleStart);
        events.OnStateChanged.RemoveListener(UpdateUI);
        events.OnHandPlayed.RemoveListener(OnHandPlayed);
        events.OnPlayerDamaged.RemoveListener(OnPlayerDamaged);
        events.OnEnemyDamaged.RemoveListener(OnEnemyDamaged);
        events.OnEnemyTurn.RemoveListener(OnEnemyTurn);
        events.OnRoundStart.RemoveListener(OnRoundStart);
        events.OnBattleEnd.RemoveListener(OnBattleEnd);
        events.OnEnergyChanged.RemoveListener(OnEnergyChanged);
        events.OnEnergyDepleted.RemoveListener(OnEnergyDepleted);
    }

    private void SetupButtons()
    {
        if (playHandButton != null)
            playHandButton.onClick.AddListener(OnPlayHandClicked);

        if (discardButton != null)
            discardButton.onClick.AddListener(OnDiscardClicked);

        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(OnEndTurnClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void CleanupButtons()
    {
        if (playHandButton != null)
            playHandButton.onClick.RemoveListener(OnPlayHandClicked);

        if (discardButton != null)
            discardButton.onClick.RemoveListener(OnDiscardClicked);

        if (endTurnButton != null)
            endTurnButton.onClick.RemoveListener(OnEndTurnClicked);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    // ==================== EVENT HANDLERS ====================

    private void OnBattleStart()
    {
        if (battleEndPanel != null)
            battleEndPanel.SetActive(false);

        var state = battleSystem.State;

        // Setup enemy info
        if (enemyIcon != null && state.Enemy.Icon != null)
            enemyIcon.sprite = state.Enemy.Icon;

        if (enemyNameText != null)
            enemyNameText.text = state.Enemy.Name;

        UpdateUI();
        AddBattleLog($"Battle started against {state.Enemy.Name}!");
        AddBattleLog($"Starting with {state.CurrentEnergy} energy");
    }

    private void UpdateUI()
    {
        var state = battleSystem.State;
        if (state == null) return;

        UpdateHPDisplay(state);
        UpdateEnergyDisplay(state);
        UpdateRoundDisplay(state);
        UpdateButtonStates(state);
    }

    private void UpdateHPDisplay(BattleState state)
    {
        // HP bars
        if (playerHPSlider != null)
            playerHPSlider.value = state.GetPlayerHPPercent();

        if (enemyHPSlider != null)
            enemyHPSlider.value = state.GetEnemyHPPercent();

        // HP text
        if (playerHPText != null)
            playerHPText.text = $"{state.PlayerHP}/{state.PlayerMaxHP}";

        if (enemyHPText != null)
            enemyHPText.text = $"{state.EnemyHP}/{state.EnemyMaxHP}";
    }

    private void UpdateEnergyDisplay(BattleState state)
    {
        if (energyText != null)
            energyText.text = $"Energy: {state.CurrentEnergy}/{state.MaxEnergy}";

        if (energySlider != null)
            energySlider.value = state.GetEnergyPercent();
    }

    private void UpdateRoundDisplay(BattleState state)
    {
        if (roundText != null)
            roundText.text = $"Round {state.RoundNumber}";
    }

    private void UpdateButtonStates(BattleState state)
    {
        bool isPlayerTurn = state.IsPlayerTurn && !battleSystem.IsProcessing;
        bool hasSelection = handSelector != null && handSelector.SelectedCards.Count > 0;
        bool canAffordPlay = battleSystem.CanAffordPlay();
        bool canAffordDiscard = battleSystem.CanAffordDiscard();

        if (playHandButton != null)
            playHandButton.interactable = isPlayerTurn && hasSelection && canAffordPlay;

        if (discardButton != null)
            discardButton.interactable = isPlayerTurn && hasSelection && canAffordDiscard;

        if (endTurnButton != null)
            endTurnButton.interactable = isPlayerTurn;
    }

    private void OnHandPlayed(PokerHandType handType, int damage)
    {
        AddBattleLog($"Played <color=yellow>{handType}</color> for <color=red>{damage}</color> damage!");
    }

    private void OnPlayerDamaged(int damage, string source)
    {
        AddBattleLog($"<color=red>Player took {damage} damage</color> from {source}");
    }

    private void OnEnemyDamaged(int damage, string source)
    {
        AddBattleLog($"<color=green>Enemy took {damage} damage!</color>");
    }

    private void OnEnemyTurn()
    {
        AddBattleLog("<color=orange>Enemy's turn...</color>");
    }

    private void OnRoundStart(int roundNumber)
    {
        AddBattleLog($"<color=cyan>=== Round {roundNumber} ===</color>");
    }

    private void OnEnergyChanged(int current, int max)
    {
        UpdateUI(); // Refresh all UI when energy changes
    }

    private void OnEnergyDepleted()
    {
        AddBattleLog("<color=yellow>Not enough energy!</color>");
    }

    private void OnBattleEnd(bool isVictory)
    {
        if (battleEndPanel != null)
            battleEndPanel.SetActive(true);

        if (battleResultText != null)
        {
            battleResultText.text = isVictory ? "VICTORY!" : "DEFEAT...";
            battleResultText.color = isVictory ? Color.green : Color.red;
        }

        AddBattleLog(isVictory ?
            "<color=green><b>VICTORY!</b></color>" :
            "<color=red><b>DEFEAT...</b></color>");
    }

    // ==================== BUTTON HANDLERS ====================

    private void OnPlayHandClicked()
    {
        if (handSelector == null || handSelector.SelectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected!");
            return;
        }

        List<Card> selectedCards = new List<Card>(handSelector.SelectedCards);
        battleSystem.PlayHand(selectedCards);

        handSelector.ClearSelection();
    }

    private void OnDiscardClicked()
    {
        if (handSelector == null || handSelector.SelectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected!");
            return;
        }

        List<Card> selectedCards = new List<Card>(handSelector.SelectedCards);
        battleSystem.DiscardCards(selectedCards);

        handSelector.ClearSelection();
    }

    private void OnEndTurnClicked()
    {
        battleSystem.EndTurn();
    }

    private void OnContinueClicked()
    {
        if (battleSystem.State.IsPlayerVictory)
        {
            // Trigger victory flow
            Debug.Log("Battle won - returning to map");
            // TODO: BattleManager.Instance.OnBattleWin();
        }
        else
        {
            // Trigger defeat flow
            Debug.Log("Battle lost - game over or retry");
            // TODO: GameManager.Instance.GameOver();
        }
    }

    // ==================== BATTLE LOG ====================

    private void AddBattleLog(string message)
    {
        if (battleLogText == null) return;

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        battleLogText.text = $"[{timestamp}] {message}\n" + battleLogText.text;

        // Limit log lines
        string[] lines = battleLogText.text.Split('\n');
        if (lines.Length > maxLogLines)
        {
            battleLogText.text = string.Join("\n", lines, 0, maxLogLines);
        }
    }

    // ==================== DEBUG ====================

    [ContextMenu("Debug - Force Win")]
    private void DebugForceWin()
    {
        battleSystem.ForceBattleEnd(true);
    }

    [ContextMenu("Debug - Force Lose")]
    private void DebugForceLose()
    {
        battleSystem.ForceBattleEnd(false);
    }

    [ContextMenu("Debug - Add Energy")]
    private void DebugAddEnergy()
    {
        if (battleSystem.State != null)
        {
            battleSystem.State.RestoreEnergy(1);
            battleSystem.Events.TriggerEnergyChanged(
                battleSystem.State.CurrentEnergy,
                battleSystem.State.MaxEnergy);
            UpdateUI();
        }
    }
}
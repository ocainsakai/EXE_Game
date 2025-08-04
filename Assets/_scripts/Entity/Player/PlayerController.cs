using Ain;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CoinRSO coinRSO;
    [SerializeField] public CardList deckList;
    [SerializeField] public BattleManager _battleManager;
    [SerializeField] public PlayerHand handController;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private PlayerStateMachine _sm;
    [SerializeField] private Button[] actionButtons;
    public Health _health { get; private set; }

    private void Awake()
    {
        InitiializeActionBtn();
        _health = new Health(_healthComponent);
        _healthComponent.Initialize(100, 0);
        _sm = GetComponent<PlayerStateMachine>();
        _sm.Initialiez(this, _battleManager);
        _sm.ChangeState(_sm.action);

    }
    internal void TurnOnAction()
    {
        foreach (var button in actionButtons)
        {
            button.enabled = true;
        }
    }
    public void TurnOffAction()
    {
        foreach (var button in actionButtons)
        {
            button.enabled = false;
        }
    }
    private void InitiializeActionBtn()
    {
        actionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "PLAY";
        actionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "DISCARD";
        actionButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "SORT";
        actionButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = "SKIP";

        actionButtons[0].onClick.AddListener(() => Attack());
        actionButtons[1].onClick.AddListener(() => handController.Discard(handController.Hand));
        actionButtons[2].onClick.AddListener(() => handController.Sort());
        //actionButtons[3].onClick.AddListener(() );

        TurnOffAction();
    }

    public void Initialize(BattleManager battleManager)
    {
        _battleManager = battleManager;
        deckList = battleManager.deckList;
        handController.Initialize(this);
        handController.DrawCard(deckList);
    }
    public void StartTurn()     {
        _sm.ChangeState(_sm.action);
    }
    public void Attack()
    {
        _sm.ChangeState(_sm.attackPhase);
    }

    public void EndTurn()
    {
        _battleManager.CheckCondition();
    }
}
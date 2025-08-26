
using System;
using UnityEngine;
public class PlayerController : ManualSingleton<PlayerController>, ICanTakeDamege
{
    [SerializeField] private PlayerButton input;
    [SerializeField] private Health playerHealth;
    [SerializeField] private DeckManager deckManager;


    private PokerHandEvaluator evaluator = new PokerHandEvaluator();
    private PlayerConfig playerConfig;  
    public Health PlayerHealth => playerHealth;
    public PlayerState playerState { get; private set; }

    public Action onPlayerEndTurn;

    private void OnEnable()
    {
        input.onDiscardButtonClicked += Discard;
        input.onPlayButtonClicked += Play;
        input.onSortButtonClicked += Sort;

        TurnManager.onRoundChanged += OnRoundChangedHandle;
        //handController.onChanged += UpdatePoker;
    }

    private void OnRoundChangedHandle()
    {
        Discard();
    }

    private void OnDisable()
    {
        input.onDiscardButtonClicked -= Discard;
        input.onPlayButtonClicked -= Play;
        input.onSortButtonClicked -= Sort;

        //handController.onChanged -= UpdatePoker;
    }

    public void LoadPlayerConfig(PlayerConfig playerConfig)
    {
        this.playerConfig = playerConfig;
        playerHealth.Init(playerConfig.MaxHp, playerConfig.MaxHp);
    }
    public async void BuidDeck()
    {
        deckManager.BuidDeck(playerConfig.StartingDeck);
        await deckManager.DrawCards();
    }
   
    private async void Sort()
    {
        await deckManager.Sort(true);
    }

    private void Play()
    {
        // resolve attack
        Debug.Log("Attack");
        EnemyManager.Instance.TakeDamege(50);
        // end
        EndTurn();

    }

    private async void Discard()
    {
        Debug.Log("DISCARD");
        await deckManager.Discards();
        await deckManager.DrawCards();
    }


    public void TakeDamege(int damege)
    {
    }

    public void Heal(int heal)
    {
    }
    public void EndTurn()
    {
        onPlayerEndTurn?.Invoke();
    }
}
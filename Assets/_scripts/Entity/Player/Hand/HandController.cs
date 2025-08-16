using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] private PlayerButton buttons;
    [SerializeField] private CardFactory cardFactory;
    [SerializeField] private CardContainer container;
    [SerializeField] private CardList hand;
    [SerializeField] private CardListSO deckList;
    [SerializeField] private PokerData HUD;

    private HandEvaluator handEvaluator;
    private SequenceQueue sequenceQueue;
    private int currentSortType = 1;
    private int HandSize = 8;

    public void Initialize()
    {
        handEvaluator = new(HUD);
        sequenceQueue = new SequenceQueue();
        hand.Clear();
        buttons.SetupActionButtons();
        buttons.OnDiscardButtonClicked += async () => await Discard();
        buttons.OnPlayButtonClicked += async () => await PlayerAttack();
        buttons.OnSortButtonClicked += async () => await Sort();
    }

    public async UniTask DrawCard(CardListSO deck)
    {
        Debug.Log("draw");
        int amount = HandSize - hand.Count;
            if (amount <= 0 || deck.cards.Count < amount) return;
        for (int i = 0; i < amount; i++)
        {
            if (deck.cards.Count == 0) break;
            Card card = deck.cards[0];
            hand.Add(card, () => handEvaluator.UpdateHUD(hand.TakeSelected()));
            deck.cards.RemoveAt(0);
        }
        foreach (Card card in hand)
        {
            SetupCard(card);
        }
        await Sort(currentSortType);
    }

    private void SetupCard(Card card)
    {
        card.transform.SetParent(container.transform);
        card.gameObject.SetActive(true);
        card.TurnOnInput();
        card.selecter.isSelected.Subscribe(x => SelectCondition());
    }

    private void SelectCondition()
    {
        SetCanSelect(hand.TakeSelected().Count < 5);
    }
    private void SetCanSelect(bool obj)
    {
        foreach (var item in hand)
        {
            item.selecter.CanSelect = obj;
        }
    }

    public async UniTask Discard()
    {
        var cards = hand.TakeSelected();
        hand.ClearSelected();
        cardFactory.ReturnCards(cards);
        handEvaluator.UpdateHUD(hand.TakeSelected());
        await  DrawCard(deckList);
    }
    public async UniTask Sort()
    {
        currentSortType = -currentSortType;
        await Sort(currentSortType);
    }
    private async UniTask Sort(int sortType)
    {
        if (sortType > 0)
        {
            hand.SortByRank();
        }
        else
        {
            hand.SortBySuit();
        }
        hand.ForEach(x => x.transform.SetSiblingIndex(hand.Count - 1));
        await container.RepositionChilds();
    }

    public async UniTask BattleStart()
    {
        Initialize();
        await UniTask.CompletedTask;
    }
    public async UniTask PlayerAction()
    {
        if (hand.TakeSelected().Count > 0)
            await Discard();
        else 
            await DrawCard(deckList);
        buttons.EnableAllActions();
        SetCanSelect(true);
    }

    public async UniTask PlayerAttack()
    {
        Debug.Log("player attack");
        SetCanSelect(false);
        buttons.DisableAllActions();
        await Attack();
    }
    public async UniTask Attack()
    {
        if (hand.Count == 0)
        {
            Debug.LogWarning("No cards selected for attack.");
            return;
        }

        var cards = hand.TakeSelected();

        Debug.Log($"{cards.Count} cards");

        foreach (var item in cards)
        {
            Debug.Log(sequenceQueue);
           
                var enemy = enemyManager.GetTargetEnemy();
                if (enemy != null)
                {
                    await enemy.TakeDamage(item.Data.Rank * HUD.PokerMult.Value);
                }
        }

        await BattleManager.Instance.CheckCondition();
    }

}

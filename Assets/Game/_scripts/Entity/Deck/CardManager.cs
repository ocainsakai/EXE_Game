using Cysharp.Threading.Tasks;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private CardListSO deckList;
    [SerializeField] private CardDatabase standardCards;
    [SerializeField] private CardFactory cardFactory;
 
    public async UniTask BattleStart()
    {
        InitializeDeck();
        deckList.cards.Shuffle();
        await UniTask.CompletedTask;
    }


    public void InitializeDeck()
    {
        var cards = cardFactory.GetCards(standardCards.Cards);
        deckList.cards.Clear();
        deckList.cards.AddRange(cards);
    }

}

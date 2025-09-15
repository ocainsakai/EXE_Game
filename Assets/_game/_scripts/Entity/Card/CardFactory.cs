using CardSystem;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private CardController cardPrefab; // prefab UI
    [SerializeField] private Transform parent;
    [SerializeField] private Transform discardPile;

    public CardController CreateCard(CardData data)
    {
        CardController card = Instantiate(cardPrefab, parent);

        card.SetData(data, discardPile);

        return card;
    }
}

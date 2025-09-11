using CardSystem;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private CardController cardPrefab; // prefab UI
    [SerializeField] private Transform parent;

    public CardController CreateCard(CardData data)
    {
        CardController card = Instantiate(cardPrefab, parent);

        card.SetData(data);

        return card;
    }
}

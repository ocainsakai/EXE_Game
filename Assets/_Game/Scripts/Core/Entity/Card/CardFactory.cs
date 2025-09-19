using CardSystem;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private Card cardPrefab; // prefab UI
    [SerializeField] private Transform parent;
    [SerializeField] public Transform discardPile;

    public Card CreateCard(CardData data)
    {
        Card card = Instantiate(cardPrefab, parent);

        card.SetData(data);

        return card;
    }
}

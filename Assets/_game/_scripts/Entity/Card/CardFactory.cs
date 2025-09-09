using CardSystem;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [SerializeField] private CardView cardPrefab; // prefab UI
    [SerializeField] private Transform parent;

    public CardView CreateCard(CardData data)
    {
        CardView card = Instantiate(cardPrefab, parent);

        card.SetData(data);

        return card;
    }
}

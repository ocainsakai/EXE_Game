using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class CardHorizontal : MonoBehaviour
{
    [SerializeField] private CardContainer _cardContainer;

    public enum Alignment
    {
        None,
        Left,
        Center,
        Right
    }

    [Header("Layout Settings")]
    public Alignment alignment = Alignment.Center;
    public int totalWidth = 1000;
    public int height = 300;
    public float spacing = 50f; // Khoảng cách giữa các thẻ

    private void Awake()
    {
        if (_cardContainer == null)
        {
            _cardContainer = GetComponent<CardContainer>();
        }

        // Theo dõi sự kiện thêm/xóa thẻ
        _cardContainer.Cards.ObserveAdd().Subscribe(card => OnCardAdded(card)).AddTo(this);
        _cardContainer.Cards.ObserveRemove().Subscribe(card => OnCardRemoved(card)).AddTo(this);
        _cardContainer.Cards.ObserveCountChanged().Subscribe(_ => UpdateLayout()).AddTo(this);
    }

    private void OnCardAdded(CollectionAddEvent<Card> cardEvent)
    {
        Card card = cardEvent.Value;
        if (card != null)
        {
            card.transform.SetParent(transform);
            card.transform.localScale = Vector3.one;
            UpdateLayout();
        }
    }

    private void OnCardRemoved(CollectionRemoveEvent<Card> cardEvent)
    {
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        if (_cardContainer == null || _cardContainer.Cards.Count == 0)
            return;

        // Tính toán vị trí các thẻ
        Vector3[] positions = CalculateCardPositions();

        // Áp dụng vị trí cho từng thẻ
        for (int i = 0; i < _cardContainer.Cards.Count; i++)
        {
            if (_cardContainer.Cards[i] != null)
            {
                RectTransform cardRect = _cardContainer.Cards[i].GetComponent<RectTransform>();
                if (cardRect != null)
                {
                    cardRect.DOAnchorPos( positions[i], 0.25f).SetEase(Ease.Linear);
                }
            }
        }
    }

    private Vector3[] CalculateCardPositions()
    {
        int cardCount = _cardContainer.Cards.Count;
        Vector3[] positions = new Vector3[cardCount];

        // Nếu alignment = None → giữ nguyên vị trí của CardContainer
        if (alignment == Alignment.None)
        {
            Vector3 basePos = transform.position;
            for (int i = 0; i < cardCount; i++)
            {
                positions[i] = basePos;
            }
            return positions;
        }

        // Tính tổng chiều rộng của tất cả thẻ + khoảng cách
        float width = (totalWidth - ((cardCount - 1) * spacing)) / cardCount;

        // Vị trí bắt đầu phụ thuộc vào căn chỉnh
        float startX = 0f;

        switch (alignment)
        {
            case Alignment.Left:
                startX = 0;
                break;

            case Alignment.Center:
                startX = -totalWidth / 2f + width / 2f;
                break;

            case Alignment.Right:
                startX = -totalWidth + width;
                break;
        }

        // Tính vị trí từng thẻ
        for (int i = 0; i < cardCount; i++)
        {
            positions[i] = new Vector3(
                startX + i * (width + spacing),
                0,
                0
            );
        }

        return positions;
    }

}
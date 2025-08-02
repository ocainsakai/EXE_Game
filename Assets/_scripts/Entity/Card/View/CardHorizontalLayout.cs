using UnityEngine;

public class CardHorizontalLayout : ICardLayoutBase
{
    private CardLayoutSettings _layoutSettings;

    private CardContainer _container;

    public CardHorizontalLayout(CardContainer container, CardLayoutSettings cardLayoutSettings)
    {
        _container = container;
        _layoutSettings = cardLayoutSettings;
    }

    public Vector3[] CalculatePositions()
    {
        if (_layoutSettings == null)
        {
            Debug.LogError("Layout Settings not assigned!");
            return new Vector3[0];
        }
        int childCount = _container.Cards.Count;
        Vector3[] positions = new Vector3[childCount];

        if (_layoutSettings.alignment == Alignment.None || childCount == 0)
            return positions;

        float itemWidth = (_layoutSettings.totalWidth - ((childCount - 1) * _layoutSettings.spacing)) / childCount;
        float halfTotalWidth = _layoutSettings.totalWidth * 0.5f;
        float halfItemWidth = itemWidth * 0.5f;

        float startX = _layoutSettings.alignment switch
        {
            Alignment.Left => 0,
            Alignment.Center => -halfTotalWidth + halfItemWidth,
            Alignment.Right => -_layoutSettings.totalWidth + itemWidth,
            _ => 0
        };

        float step = itemWidth + _layoutSettings.spacing;
        for (int i = 0; i < childCount; i++)
        {
            positions[i] = new Vector3(startX + i * step, 0, 0);
        }

        return positions;
    }

    public void ApplyLayout(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length && i < _container.Cards.Count; i++)
        {
            _container.Cards[i].transform.localPosition = positions[i];
        }
    }

    public void UpdateLayout()
    {
        ApplyLayout(CalculatePositions());
    }
}
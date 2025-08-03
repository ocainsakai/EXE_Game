using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CardSelecter : MonoBehaviour, ISelectable
{
    public bool CanSelect { get; set; } = false;
    public ReactiveProperty<bool> isSelected = new ReactiveProperty<bool>(false);
    public bool IsSelected => isSelected.Value;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponentInChildren<Image>().GetComponent<RectTransform>();
        _originalPosition = Vector2.zero;

        GetComponent<CardInputDecider>().OnClick += OnClicked;
    }

    public void OnClicked()
    {
        if (CanSelect && !IsSelected)
        {
            Select();
        }
        else if (IsSelected)
        {
            Deselect();
        }
    }

    public void Select()
    {
        isSelected.Value = true;
        _rectTransform.DOLocalMoveY(_originalPosition.y + 40f, 0.2f)
            .SetEase(Ease.OutBack);
    }

    public void Deselect()
    {
        isSelected.Value = false;
        _rectTransform.DOLocalMoveY(_originalPosition.y, 0.2f)
            .SetEase(Ease.OutBack);
    }

    // Reset position khi bị disable
    private void OnDisable()
    {
        _rectTransform.anchoredPosition = _originalPosition;
        isSelected.Value = false;
    }
}
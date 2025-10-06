using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardEntry : MonoBehaviour
{
    [SerializeField] private Image _art;
    [SerializeField] private Button _selectBtn;

    public SerializableGuid CardID;
    public Card Card {  get; private set; }
    public bool IsSelected { get; private set; }
    public Action OnCardClicked;

    private bool CanSelect;
    public void SetCard(Card card)
    {
        CardID = card.CardID;
        CanSelect = true;
        IsSelected = false;
        this.Card = card;
        if (_art != null)
        {
            _art.sprite = card.Art;
        }
    }

    public void SetButton(bool canSelect)
    {
        CanSelect = canSelect;
    }
    private void OnEnable()
    {
        if (_selectBtn != null)
        {
            _selectBtn.onClick.AddListener(() => {
                OnSelected();
                Card.IsSeleced = IsSelected;
            });
        }
    }
    private void OnDisable()
    {
        if(_selectBtn != null)
        {
            _selectBtn.onClick.RemoveAllListeners();
        }
    }

    public void OnSelected()
    {
        OnCardClicked?.Invoke();
        if (!CanSelect && !IsSelected) return;
        IsSelected = !IsSelected;
        transform.DOLocalMoveY((IsSelected ? 50 : 0), 0.1f);
    }
}

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardEntry : MonoBehaviour
{
    [SerializeField] private Image _art;
    [SerializeField] private Button _selectBtn;

    public Action OnCardClick;
    public void SetImage(Sprite sprite)
    {
        if (_art != null)
        {
            _art.sprite = sprite;
        }
    }

    private void OnEnable()
    {
        if (_selectBtn != null)
        {
            _selectBtn.onClick.AddListener(() => OnCardClick?.Invoke());
        }
    }
    private void OnDisable()
    {
        if(OnCardClick != null)
        {
            _selectBtn.onClick.RemoveAllListeners();
        }
    }

    public void OnSelected(bool IsSelect)
    {
        transform.DOLocalMoveY((IsSelect ? 50 : 0), 0.1f);
    }
}

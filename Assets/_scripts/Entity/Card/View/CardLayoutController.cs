using UnityEngine;
using UniRx;
using DG.Tweening;

public class CardLayoutController : MonoBehaviour
{
    [SerializeField] private CardContainer _cardContainer;
    [SerializeField] private bool _animateLayout = true;
    [SerializeField] private float _moveDuration = 0.25f;
    [SerializeField] private Ease _moveEase = Ease.Linear;
    [SerializeField] private CardLayoutSettings _layoutSettings;

    private ICardLayoutBase _layout;
    private CardSiblingOrder _siblingOrder;
    private Subject<Unit> _layoutUpdateSubject = new Subject<Unit>();
    private Sequence _currentSequence;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Khởi tạo các thành phần
        _siblingOrder = new CardSiblingOrder(_cardContainer);
        _layout = new CardHorizontalLayout(_cardContainer, _layoutSettings);
        // Thiết lập observer
        _cardContainer.Cards.ObserveAdd().Subscribe(_ => RequestLayoutUpdate()).AddTo(this);
        _cardContainer.Cards.ObserveRemove().Subscribe(_ => RequestLayoutUpdate()).AddTo(this);
        _cardContainer.Cards.ObserveReplace().Subscribe(_ => RequestLayoutUpdate()).AddTo(this);
        _layoutUpdateSubject
            .ThrottleFrame(1)
            .Subscribe(_ => UpdateAllLayouts())
            .AddTo(this);
    }
    public void UpdateOrder() => _siblingOrder.UpdateOrder();
    public void RequestLayoutUpdate()
    {
        _layoutUpdateSubject.OnNext(Unit.Default);
    }

    private void UpdateAllLayouts()
    {
        UpdateVisualLayout();
        _siblingOrder.UpdateOrder();
    }

    private void UpdateVisualLayout()
    {
        var positions = _layout.CalculatePositions();

        if (_animateLayout)
        {
            AnimateLayout(positions);
        }
        else
        {
            _layout.ApplyLayout(positions);
        }
    }

    private void AnimateLayout(Vector3[] positions)
    {
        ClearCurrentSequence();

        _currentSequence = DOTween.Sequence();
        _currentSequence.SetEase(_moveEase);

        for (int i = 0; i < positions.Length && i < transform.childCount; i++)
        {
            Transform child = _cardContainer.Cards[i].transform;
            _currentSequence.Join(
                child.DOLocalMove(positions[i], _moveDuration)
            );
        }
    }

    private void ClearCurrentSequence()
    {
        if (_currentSequence != null && _currentSequence.IsActive())
        {
            _currentSequence.Kill();
        }
        _currentSequence = null;
    }

    private void OnDestroy()
    {
        ClearCurrentSequence();
        _layoutUpdateSubject?.Dispose();
    }
}
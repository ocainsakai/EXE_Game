using DG.Tweening;
using UnityEngine;

public class TitleIdle : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Tiêu đề sẽ phóng to đến mức này (ví dụ: 1.05 = 105%)")]
    [SerializeField] private float targetScale = 1.05f;

    [Tooltip("Thời gian để hoàn thành một chu kỳ (phóng to hoặc thu nhỏ)")]
    [SerializeField] private float pulseDuration = 1.5f;

    [Tooltip("Kiểu 'easing' (độ mượt) của animation")]
    [SerializeField] private Ease animationEase = Ease.InOutSine;

    private Vector3 initialScale;
    private Tween idleTween;

    void Start()
    {
        // 2. Lưu lại kích thước ban đầu
        initialScale = transform.localScale;

        // 3. Bắt đầu animation
        StartIdleAnimation();
    }

    private void StartIdleAnimation()
    {
        // 4. Tạo tween (animation)
        idleTween = transform.DOScale(initialScale * targetScale, pulseDuration)
            .SetEase(animationEase)      // Đặt độ mượt
            .SetLoops(-1, LoopType.Yoyo); // 5. Lặp lại vô hạn, kiểu Yoyo (đi rồi về)
    }

    private void OnDestroy()
    {
        // 6. (Rất quan trọng) Hủy tween khi object bị phá hủy
        // Nếu không, DOTween có thể báo lỗi khi cố gắng
        // animate một object không còn tồn tại.
        idleTween?.Kill();
        
        // Hoặc an toàn hơn:
        transform.DOKill();
    }
}

using CardSystem.PokerSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] MultTable multTable;
    [SerializeField] Room room;
    [SerializeField] TextMeshProUGUI multText;
    public UnityEvent<float> AttackEnemy;

    private float mult;
    private void Awake()
    {
        room.OnPokerHandResult.AddListener(UpdateMult);
        Card.OnActive += CardEffect;
    }

    private void UpdateMult(PokerHandResult result)
    {
        mult = multTable.GetMult(result.HandType);
        multText.text = $"{result.HandType}: x{(int)mult}";
    }
    private IEnumerator CardEffect(Card card)
    {
        //Debug.Log($"Kích hoạt hiệu ứng cho {card}");
        float dame =(int)card.Rank * mult;
        AttackEnemy?.Invoke(dame);
        yield return new WaitForSeconds(0.1f);
        //Debug.Log($"Hiệu ứng {card} kết thúc");
    }

    private void OnDestroy()
    {
        Card.OnActive -= CardEffect;

    }
}

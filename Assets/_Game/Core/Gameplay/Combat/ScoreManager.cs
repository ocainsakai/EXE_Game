using CardSystem.PokerSystem;
using System.Collections;
using _Game.Addons.Deck.Scripts;
using _Game.Addons.Deck.Scripts.Card;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] MultTable multTable;
    [SerializeField] Room room;
    [SerializeField] TextMeshProUGUI multText;
    [FormerlySerializedAs("AttackEnemy")] public UnityEvent<float> attackEnemy;

    private float mult;
    private void Awake()
    {
        room.onPokerHandResult.AddListener(UpdateMult);
    }

    private void UpdateMult(PokerHandResult result)
    {
        mult = multTable.GetMult(result.HandType);
        multText.text = $"{result.HandType}: x{(int)mult}";
    }
    public IEnumerator CardEffect(CardRuntime cardRuntime)
    {
        //Debug.Log($"Kích hoạt hiệu ứng cho {card}");
        float dame =(int)cardRuntime.Rank * mult;
        attackEnemy?.Invoke(dame);
        yield return new WaitForSeconds(0.1f);
        //Debug.Log($"Hiệu ứng {card} kết thúc");
    }

}

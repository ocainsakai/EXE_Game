using CardSystem.PokerSystem;
using System.Collections;
using System.Collections.Generic;
using _Game.Addons.Deck.Scripts.Card;
using _Game.Core.Gameplay.Combat;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] BattleManager battleManager;
    [SerializeField] BattleVFXManager battleVFXManager;
    [SerializeField] MultTable multTable;
    [SerializeField] Room room;
    [SerializeField] TextMeshProUGUI multText;

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
    public IEnumerator ActivateCards(List<CardRuntime>  cardsToPlay)
    {
        foreach (var card in cardsToPlay)
        {
            Debug.Log($"[PlayerActionController] Activating card: {card.Name}");
            battleManager.mediator.UseEnergyPlay();
            yield return battleVFXManager?.PlayAttackVFX();
            yield return CardEffect(card);
        }
        yield return new WaitForSeconds(0.5f);
    }
    public IEnumerator CardEffect(CardRuntime cardRuntime)
    {
        //Debug.Log($"Kích hoạt hiệu ứng cho {card}");
        float dame =(int)cardRuntime.Rank * mult;
        battleManager.AttackEnemy(dame);
        yield return new WaitForSeconds(1f);
        //Debug.Log($"Hiệu ứng {card} kết thúc");
    }

}

using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public EnemyManager enemyManager;
    public PlayerStat stat;
    public CardsContext cardsContext;
    public PokerManager pokerManager;   
    public GameManager gameManager;
    public void StartBattle()
    {
        foreach (var item in cardsContext.played)
        {
            if (!pokerManager.matchedCards.Contains(item))
            {
                item.SetFace(false);
            } else
            {
                var enemy = enemyManager.GetEnemy();
                int dame = item.Data.Value * pokerManager.pokerData.BaseMult;
                enemy?.TakeDame(dame);
            }
        }
        gameManager.ChangeTurn(Phase.EnemiesTurn);
        gameManager.ChangeTurn(Phase.EndTurn);
    }

}

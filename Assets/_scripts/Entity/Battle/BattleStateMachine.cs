using Ain;
using UnityEngine;

public class BattleStateMachine : StateMachine<IState>
{
    [SerializeField] CardDatabase standardCards;
    [SerializeField] EnemyDatabase combatContext;
    [SerializeField] EnemyBattleUI battleUI;
    public Player player;
    private void Start()
    {
        battleUI.InitializedEnemy(combatContext.Enemies);
        player.Initialize(standardCards.Cards);
    }


}
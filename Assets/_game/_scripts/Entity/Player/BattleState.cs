using System;
using System.Collections.Generic;
using UniRx.Diagnostics;

[Serializable]
public class BattleState
{
    public int Seed;
    public PlayerState Player;
    public EnemyState Enemy;
    public int TurnNumber;
    public string ActiveEntityId; // "player" or enemy id
    public List<LogEntry> RecentLog; // optional
}

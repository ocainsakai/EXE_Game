using System;

[Serializable]
public class EnemyState
{
    public SerializableGuid EnemyId;
    public int Hp;
    public int TurnCounter;
    public int PatternIndex;
    //public List<StatusInstance> Statuses;
}

using System;

[Serializable]
public class PlayerState
{
    public SerializableGuid Id;
    public int MaxHp;
    public int Hp;
    public int ApMax;
    public int ApCurrent;
    public int ApRegenPerTurn;
    public DeckState DeckState;
    //public List<StatusInstance> Statuses;
}

public interface  ICanTakeDamegeAdvance : ICanTakeDamege
{
    void TakeDamegeOverTime(int damege, float time);
    void TakeTrueDamege(int damege);
    void HealOverTime(int heal, float time);
}
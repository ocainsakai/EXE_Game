using System;

public interface ICanTakeDamege
{
    void TakeDamege(int damege);
    //void TakeDamege(int damege, Action onDamegeTaken);
    //void TakeDamege(int damege, object sou);
    void Heal(int heal);
}

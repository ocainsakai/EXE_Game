
using UnityEngine;

public abstract class GameUnit : MonoBehaviour
{
    public int HP;
    public int ATK;
    public abstract void Attack(GameUnit target);
}

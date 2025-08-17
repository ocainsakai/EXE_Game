using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField] private Stat energy;
    public int AmountToUse(int cardsCount)
    {
        return (cardsCount / 2) + 1; 
    }
}

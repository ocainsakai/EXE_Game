using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardList", menuName = "Scriptable Objects/CardList")]
public class CardList : ScriptableObject
{
    public List<Card> cards;
}

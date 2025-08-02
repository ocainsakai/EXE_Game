
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Data/CardData", order = 1)]
public class CardData : ScriptableObject
{
	public int Cost;
	public string Name;
	public string Description;
	
}
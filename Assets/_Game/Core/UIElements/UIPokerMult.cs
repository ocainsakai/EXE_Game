using CardSystem.PokerSystem;
using TMPro;
using UnityEngine;

public class UIPokerMult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pokerTxt;
    [SerializeField] private TextMeshProUGUI _multTxt;

    public void SetPokerMult(PokerHandType poker = PokerHandType.None, float mult = 0)
    {
        _pokerTxt.text = poker.ToString();
        _multTxt.text = $"x{(int) mult}";
    }
}

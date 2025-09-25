using CardSystem.PokerSystem;
using TMPro;
using UnityEngine;

public class UIPokerMult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pokerTxt;
    [SerializeField] private TextMeshProUGUI _multTxt;

    public void SetPokerMult(PokerHandType poker = PokerHandType.None, int mult = 0)
    {
        _pokerTxt.text = poker.ToString();
        _multTxt.text = $"x{mult}";
    }
}

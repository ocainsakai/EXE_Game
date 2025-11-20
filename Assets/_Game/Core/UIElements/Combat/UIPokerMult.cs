using CardSystem.PokerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIPokerMult : MonoBehaviour
{
    [FormerlySerializedAs("_pokerTxt")] [SerializeField] private TextMeshProUGUI pokerTxt;
    [FormerlySerializedAs("_multTxt")] [SerializeField] private TextMeshProUGUI multTxt;

    public void SetPokerMult(PokerHandType poker = PokerHandType.KhongCo, float mult = 0)
    {
        pokerTxt.text = PokerHandNames.GetDisplayName(poker);
        multTxt.text = $"x{(int) mult}";
    }
}

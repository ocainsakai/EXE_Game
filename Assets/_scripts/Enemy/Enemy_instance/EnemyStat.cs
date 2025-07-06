using UnityEngine;
using UniRx;
using TMPro;
public class EnemyStat : MonoBehaviour
{
    public TextMeshPro turnText;
    public int TurnsToAction = 3;
    public ReactiveProperty<int> TurnsRemain = new();

    private void Awake()
    {
        TurnsRemain.Subscribe(x => turnText.text = $"{x}/{TurnsToAction}");
        TurnsRemain.Value = TurnsToAction;
    }
    public void CountToAction()
    {
        TurnsRemain.Value--;
        if (TurnsRemain.Value == 0) Action();
    }
    public void Action()
    {
        TurnsRemain.Value = TurnsToAction;
    }
}

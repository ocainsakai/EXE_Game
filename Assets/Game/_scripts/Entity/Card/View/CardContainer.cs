using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] CardLayoutSettings settings;
    public async UniTask RepositionChilds()
    {
        var tasks = new UniTask[transform.childCount];
        var positions = CalculatePosition();
        foreach(Transform child in transform)
        {
            int index = child.GetSiblingIndex();
            Vector3 targetPosition = positions[index];
            tasks[index] = child.DOLocalMove(targetPosition, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
        }
        await UniTask.WhenAll(tasks);
    } 

    private Vector3[] CalculatePosition()
    {
        int childCount = transform.childCount;
        Vector3[] positions = new Vector3[childCount];
        float totalWidth = settings.totalWidth;
        float spacing = settings.spacing;
        float startX = - Mathf.Min(totalWidth/2,spacing * childCount / 2);
        for (int i = 0; i < childCount; i++)
        {
            float x = startX + i * (spacing);
            positions[i] = new Vector3(x, 0, 0);
        }
        return positions;
    }
}

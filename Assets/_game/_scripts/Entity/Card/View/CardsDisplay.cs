using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class CardsDisplay : MonoBehaviour
{
    [SerializeField] HandController controller;
    [SerializeField] CardLayoutSettings settings;

    public async UniTask OnCountChangedHandle()
    {
        for (int i = 0; i < controller.Count; i++) {
            var card = controller[i];
            card.transform.SetParent(transform, false);
            card.transform.SetSiblingIndex(i);
        }
        await RepositionChilds();
    }

    public async UniTask RepositionChilds()
    {
        var tasks = new UniTask[transform.childCount];
        var positions = CalculatePosition();
        for(int i = 0; i<transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Vector3 targetPosition = positions[i];
            tasks[i] = child.DOLocalMove(targetPosition, 0.25f).SetEase(Ease.Linear).AsyncWaitForCompletion().AsUniTask();
        }
        await UniTask.WhenAll(tasks);
    } 

    private Vector3[] CalculatePosition(int? count = null)
    {
        int childCount = count ?? transform.childCount;
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

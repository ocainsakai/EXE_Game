using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TweenManager
{
    public static TweenManager PlayerAttackTween = new TweenManager();
    private readonly List<UniTask> _activeTweenTasks = new List<UniTask>();
    private readonly HashSet<UniTask> _completedTasks = new HashSet<UniTask>();

    public void AddTweenTask(UniTask tweenTask)
    {
        var task = tweenTask.ContinueWith(() =>
        {
            _completedTasks.Add(tweenTask);
        });
        _activeTweenTasks.Add(task);
    }

    public async UniTask WaitForAllTweens()
    {
        // Lọc ra các task chưa hoàn thành
        var activeTasks = _activeTweenTasks.Where(t => !_completedTasks.Contains(t)).ToList();

        if (activeTasks.Count > 0)
        {
            await UniTask.WhenAll(activeTasks);
        }

        _activeTweenTasks.Clear();
        _completedTasks.Clear();
    }

    public void CancelAllTweens()
    {
        // Với UniTask, thường hủy thông qua CancellationToken
        // Bạn cần thiết kế các tween task hỗ trợ cancellation
        _activeTweenTasks.Clear();
        _completedTasks.Clear();
    }
}
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SequenceQueue
{
    private Queue<Func<UniTask>> _queue = new Queue<Func<UniTask>>();
    private bool _isProcessing;

    public void Enqueue(Func<UniTask> taskFunc)
    {
        Debug.Log(_queue.Count);
        _queue.Enqueue(taskFunc);

        if (!_isProcessing)
        {
            _ = ProcessQueue();
        }
    }
    public async UniTask EnqueueAsync(Func<UniTask> taskFunc)
    {
        _queue.Enqueue(taskFunc);

        if (!_isProcessing)
        {
            await ProcessQueue();
        }
    }
    private async UniTask ProcessQueue()
    {
        _isProcessing = true;

        try
        {
            while (_queue.Count > 0)
            {
                var task = _queue.Dequeue();
                await task.Invoke();
            }
        }
        finally
        {
            _isProcessing = false;
        }
    }
}
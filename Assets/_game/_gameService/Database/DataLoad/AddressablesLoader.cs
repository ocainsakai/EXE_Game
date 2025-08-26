using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;

public class AddressablesLoader<T> : IDataLoader<T> where T : class
{
    public async UniTask<T> Load(string key, CancellationToken ct)
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.WithCancellation(ct);
        return handle.Result;
    }

    public async UniTask Preload(string key, CancellationToken ct)
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.WithCancellation(ct);
    }

    public void Release(T resource)
    {
        Addressables.Release(resource);
    }
}

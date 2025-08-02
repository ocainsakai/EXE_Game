using Cysharp.Threading.Tasks;
using System.Threading;

public interface IDataLoader<T>
{
    UniTask<T> Load(string key, CancellationToken ct);
    UniTask Preload(string key, CancellationToken ct);
    void Release(T resource);
}

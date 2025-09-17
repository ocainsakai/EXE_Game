using System.Collections.Generic;
using UnityEngine;

public interface IHex
{
    public Vector2Int HexPosition { get; }
    public void SetHexPosition(Vector2Int position);
}

public interface IHex<T> : IHex where T : IHex<T>
{
    public T GetNeighbor(HexDirection direction);
    public T GetNeighbor(int direction);
}

public interface IHexPathfinder<T> where T : IHex<T>
{
    List<T> FindPath(T start, T goal);
}
public interface IHexMap<T> where T : IHex<T>
{
    T GetHexAt(Vector2Int position);
    bool IsValidHex(Vector2Int position);
}
public interface IHexMapGenerator<T> where T : IHex<T>
{
    void GenerateMap(IHexLayoutGenerator layoutGenerator);
    void ClearMap();
}
public interface IHexMapRenderer<T> where T : IHex<T>
{
    void RenderMap(IHexMap<T> map);
    void ClearMap();
}
public interface IHexTile<T> where T : IHex<T>
{
    T Hex { get; }
    void SetHex(T hex);
}
public interface IHexTileRenderer<T> where T : IHex<T>
{
    void RenderTile(IHexTile<T> tile);
    void ClearTile(IHexTile<T> tile);
}
public interface IHexTileSelector<T> where T : IHex<T>
{
    void SelectTile(IHexTile<T> tile);
    void DeselectTile(IHexTile<T> tile);
}
public interface IHexTileHighlighter<T> where T : IHex<T>
{
    void HighlightTile(IHexTile<T> tile);
    void ClearHighlight(IHexTile<T> tile);
}
public interface IHexTileAnimator<T> where T : IHex<T>
{
    void AnimateTile(IHexTile<T> tile);
    void StopAnimation(IHexTile<T> tile);
}
public interface IHexTileEventHandler<T> where T : IHex<T>
{
    void OnTileClicked(IHexTile<T> tile);
    void OnTileHovered(IHexTile<T> tile);
}
public interface IHexTileDataProvider<T> where T : IHex<T>
{
    object GetData(IHexTile<T> tile);
    void SetData(IHexTile<T> tile, object data);
}
public interface IHexTileModifier<T> where T : IHex<T>
{
    void ModifyTile(IHexTile<T> tile, object modification);
}
public interface IHexTileFactory<T> where T : IHex<T>
{
    IHexTile<T> CreateTile(T hex);
    void DestroyTile(IHexTile<T> tile);
}
public interface IHexTileCache<T> where T : IHex<T>
{
    IHexTile<T> GetTile(T hex);
    void AddTile(IHexTile<T> tile);
    void RemoveTile(IHexTile<T> tile);
}
public interface IHexTileNeighborProvider<T> where T : IHex<T>
{
    IEnumerable<IHexTile<T>> GetNeighbors(IHexTile<T> tile);
}
public interface IHexTilePathfinder<T> where T : IHex<T>
{
    List<IHexTile<T>> FindPath(IHexTile<T> start, IHexTile<T> goal);
}
public interface IHexTileClusterer<T> where T : IHex<T>
{
    IEnumerable<IEnumerable<IHexTile<T>>> ClusterTiles(IEnumerable<IHexTile<T>> tiles);
}
public interface IHexTileSorter<T> where T : IHex<T>
{
    IEnumerable<IHexTile<T>> SortTiles(IEnumerable<IHexTile<T>> tiles, System.Comparison<IHexTile<T>> comparison);
}
public interface IHexTileFilter<T> where T : IHex<T>
{
    IEnumerable<IHexTile<T>> FilterTiles(IEnumerable<IHexTile<T>> tiles, System.Predicate<IHexTile<T>> predicate);
}
public interface IHexTileTransformer<T> where T : IHex<T>
{
    void TransformTile(IHexTile<T> tile, System.Func<IHexTile<T>, IHexTile<T>> transformer);
}
public interface IHexTileVisualizer<T> where T : IHex<T>
{
    void VisualizeTile(IHexTile<T> tile);
    void ClearVisualization(IHexTile<T> tile);
}
public interface IHexTileUpdater<T> where T : IHex<T>
{
    void UpdateTile(IHexTile<T> tile);
}
public interface IHexTileLoader<T> where T : IHex<T>
{
    IHexTile<T> LoadTile(Vector2Int position);
    void UnloadTile(IHexTile<T> tile);
}
public interface IHexTileSaver<T> where T : IHex<T>
{
    void SaveTile(IHexTile<T> tile);
    IHexTile<T> LoadTile(Vector2Int position);
}
public interface IHexTileManager<T> where T : IHex<T>
{
    IHexTile<T> GetTileAt(Vector2Int position);
    void AddTile(IHexTile<T> tile);
    void RemoveTile(IHexTile<T> tile);
}
public interface IHexTileCoordinator<T> where T : IHex<T>
{
    void CoordinateTiles(IEnumerable<IHexTile<T>> tiles);
}
public interface IHexTileObserver<T> where T : IHex<T>
{
    void OnTileChanged(IHexTile<T> tile);
}
public interface IHexTileMediator<T> where T : IHex<T>
{
    void MediateTile(IHexTile<T> tile);
}
public interface IHexTileStrategy<T> where T : IHex<T>
{
    void ExecuteStrategy(IHexTile<T> tile);
}
public interface IHexTileCommand<T> where T : IHex<T>
{
    void ExecuteCommand(IHexTile<T> tile);
}
public interface IHexTileVisitor<T> where T : IHex<T>
{
    void VisitTile(IHexTile<T> tile);
}
public interface IHexTileDecorator<T> where T : IHex<T>
{
    void DecorateTile(IHexTile<T> tile);
}
public interface IHexTileAdapter<T> where T : IHex<T>
{
    IHexTile<T> AdaptTile(object obj);
}
public interface IHexTileProxy<T> where T : IHex<T>
{
    IHexTile<T> GetTile();
}
public interface IHexTileBuilder<T> where T : IHex<T>
{
    IHexTile<T> BuildTile(T hex);
}
public interface IHexTileDirector<T> where T : IHex<T>
{
    IHexTile<T> ConstructTile(IHexTileBuilder<T> builder, T hex);
}
public interface IHexTileFacade<T> where T : IHex<T>
{
    void PerformOperation(IHexTile<T> tile);
}
public interface IHexTileBridge<T> where T : IHex<T>
{
    void BridgeTile(IHexTile<T> tile);
}
public interface IHexTileComposite<T> where T : IHex<T>
{
    void AddTile(IHexTile<T> tile);
    void RemoveTile(IHexTile<T> tile);
    IEnumerable<IHexTile<T>> GetTiles();
}
public interface IHexTileFlyweight<T> where T : IHex<T>
{
    IHexTile<T> GetSharedTile(T hex);
}
public interface IHexTileChainOfResponsibility<T> where T : IHex<T>
{
    void HandleTile(IHexTile<T> tile);
}
public interface IHexTileState<T> where T : IHex<T>
{
    void HandleState(IHexTile<T> tile);
}
public interface IHexTileMemento<T> where T : IHex<T>
{
    object SaveState(IHexTile<T> tile);
    void RestoreState(IHexTile<T> tile, object state);
}
public interface IHexTileIterator<T> where T : IHex<T>
{
    IEnumerator<IHexTile<T>> GetEnumerator(IEnumerable<IHexTile<T>> tiles);
}
public interface IHexTileObservable<T> where T : IHex<T>
{
    void Subscribe(IHexTileObserver<T> observer);
    void Unsubscribe(IHexTileObserver<T> observer);
    void NotifyObservers(IHexTile<T> tile);
}
public interface IHexTileEvent<T> where T : IHex<T>
{
    void TriggerEvent(IHexTile<T> tile);
}
public interface IHexTileService<T> where T : IHex<T>
{
    void PerformService(IHexTile<T> tile);
}
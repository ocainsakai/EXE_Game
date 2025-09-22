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
public interface IHexTileRenderer<T> where T : IHex<T>
{
    void RenderTile(IHexOccupant<T> tile);
    void ClearTile(IHexOccupant<T> tile);
}
public interface IHexTileSelector<T> where T : IHex<T>
{
    void SelectTile(IHexOccupant<T> tile);
    void DeselectTile(IHexOccupant<T> tile);
}
public interface IHexTileHighlighter<T> where T : IHex<T>
{
    void HighlightTile(IHexOccupant<T> tile);
    void ClearHighlight(IHexOccupant<T> tile);
}
public interface IHexTileAnimator<T> where T : IHex<T>
{
    void AnimateTile(IHexOccupant<T> tile);
    void StopAnimation(IHexOccupant<T> tile);
}
public interface IHexTileEventHandler<T> where T : IHex<T>
{
    void OnTileClicked(IHexOccupant<T> tile);
    void OnTileHovered(IHexOccupant<T> tile);
}
public interface IHexTileDataProvider<T> where T : IHex<T>
{
    object GetData(IHexOccupant<T> tile);
    void SetData(IHexOccupant<T> tile, object data);
}
public interface IHexTileModifier<T> where T : IHex<T>
{
    void ModifyTile(IHexOccupant<T> tile, object modification);
}
public interface IHexTileFactory<T> where T : IHex<T>
{
    IHexOccupant<T> CreateTile(T hex);
    void DestroyTile(IHexOccupant<T> tile);
}
public interface IHexTileCache<T> where T : IHex<T>
{
    IHexOccupant<T> GetTile(T hex);
    void AddTile(IHexOccupant<T> tile);
    void RemoveTile(IHexOccupant<T> tile);
}
public interface IHexTileNeighborProvider<T> where T : IHex<T>
{
    IEnumerable<IHexOccupant<T>> GetNeighbors(IHexOccupant<T> tile);
}
public interface IHexTilePathfinder<T> where T : IHex<T>
{
    List<IHexOccupant<T>> FindPath(IHexOccupant<T> start, IHexOccupant<T> goal);
}
public interface IHexTileClusterer<T> where T : IHex<T>
{
    IEnumerable<IEnumerable<IHexOccupant<T>>> ClusterTiles(IEnumerable<IHexOccupant<T>> tiles);
}
public interface IHexTileSorter<T> where T : IHex<T>
{
    IEnumerable<IHexOccupant<T>> SortTiles(IEnumerable<IHexOccupant<T>> tiles, System.Comparison<IHexOccupant<T>> comparison);
}
public interface IHexTileFilter<T> where T : IHex<T>
{
    IEnumerable<IHexOccupant<T>> FilterTiles(IEnumerable<IHexOccupant<T>> tiles, System.Predicate<IHexOccupant<T>> predicate);
}
public interface IHexTileTransformer<T> where T : IHex<T>
{
    void TransformTile(IHexOccupant<T> tile, System.Func<IHexOccupant<T>, IHexOccupant<T>> transformer);
}
public interface IHexTileVisualizer<T> where T : IHex<T>
{
    void VisualizeTile(IHexOccupant<T> tile);
    void ClearVisualization(IHexOccupant<T> tile);
}
public interface IHexTileUpdater<T> where T : IHex<T>
{
    void UpdateTile(IHexOccupant<T> tile);
}
public interface IHexTileLoader<T> where T : IHex<T>
{
    IHexOccupant<T> LoadTile(Vector2Int position);
    void UnloadTile(IHexOccupant<T> tile);
}
public interface IHexTileSaver<T> where T : IHex<T>
{
    void SaveTile(IHexOccupant<T> tile);
    IHexOccupant<T> LoadTile(Vector2Int position);
}
public interface IHexTileManager<T> where T : IHex<T>
{
    IHexOccupant<T> GetTileAt(Vector2Int position);
    void AddTile(IHexOccupant<T> tile);
    void RemoveTile(IHexOccupant<T> tile);
}
public interface IHexTileCoordinator<T> where T : IHex<T>
{
    void CoordinateTiles(IEnumerable<IHexOccupant<T>> tiles);
}
public interface IHexTileObserver<T> where T : IHex<T>
{
    void OnTileChanged(IHexOccupant<T> tile);
}
public interface IHexTileMediator<T> where T : IHex<T>
{
    void MediateTile(IHexOccupant<T> tile);
}
public interface IHexTileStrategy<T> where T : IHex<T>
{
    void ExecuteStrategy(IHexOccupant<T> tile);
}
public interface IHexTileCommand<T> where T : IHex<T>
{
    void ExecuteCommand(IHexOccupant<T> tile);
}
public interface IHexTileVisitor<T> where T : IHex<T>
{
    void VisitTile(IHexOccupant<T> tile);
}
public interface IHexTileDecorator<T> where T : IHex<T>
{
    void DecorateTile(IHexOccupant<T> tile);
}
public interface IHexTileAdapter<T> where T : IHex<T>
{
    IHexOccupant<T> AdaptTile(object obj);
}
public interface IHexTileProxy<T> where T : IHex<T>
{
    IHexOccupant<T> GetTile();
}
public interface IHexTileBuilder<T> where T : IHex<T>
{
    IHexOccupant<T> BuildTile(T hex);
}
public interface IHexTileDirector<T> where T : IHex<T>
{
    IHexOccupant<T> ConstructTile(IHexTileBuilder<T> builder, T hex);
}
public interface IHexTileFacade<T> where T : IHex<T>
{
    void PerformOperation(IHexOccupant<T> tile);
}
public interface IHexTileBridge<T> where T : IHex<T>
{
    void BridgeTile(IHexOccupant<T> tile);
}
public interface IHexTileComposite<T> where T : IHex<T>
{
    void AddTile(IHexOccupant<T> tile);
    void RemoveTile(IHexOccupant<T> tile);
    IEnumerable<IHexOccupant<T>> GetTiles();
}
public interface IHexTileFlyweight<T> where T : IHex<T>
{
    IHexOccupant<T> GetSharedTile(T hex);
}
public interface IHexTileChainOfResponsibility<T> where T : IHex<T>
{
    void HandleTile(IHexOccupant<T> tile);
}
public interface IHexTileState<T> where T : IHex<T>
{
    void HandleState(IHexOccupant<T> tile);
}
public interface IHexTileMemento<T> where T : IHex<T>
{
    object SaveState(IHexOccupant<T> tile);
    void RestoreState(IHexOccupant<T> tile, object state);
}
public interface IHexTileIterator<T> where T : IHex<T>
{
    IEnumerator<IHexOccupant<T>> GetEnumerator(IEnumerable<IHexOccupant<T>> tiles);
}
public interface IHexTileObservable<T> where T : IHex<T>
{
    void Subscribe(IHexTileObserver<T> observer);
    void Unsubscribe(IHexTileObserver<T> observer);
    void NotifyObservers(IHexOccupant<T> tile);
}
public interface IHexTileEvent<T> where T : IHex<T>
{
    void TriggerEvent(IHexOccupant<T> tile);
}
public interface IHexTileService<T> where T : IHex<T>
{
    void PerformService(IHexOccupant<T> tile);
}
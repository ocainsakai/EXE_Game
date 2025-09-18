using UnityEngine;

public class OccupantFactory : MonoBehaviour
{
    [SerializeField] 

    private OccupantPrefabEntry prefabEntries;



    public IHexOccupant<Hex> CreateOccupant(HexContentType type, Transform parent, Vector3 position)
    {
        var occupantPrefab = prefabEntries.occupantPrefabs.Find(x => x.type == type)?.prefabs;
        if (occupantPrefab == null)
            return null;
        switch (type)
        {
            case HexContentType.Player:
                var playerPrefab = occupantPrefab[Random.Range(0, occupantPrefab.Count)];
                var playerInstance = Instantiate(playerPrefab, position, Quaternion.identity, parent);
                return playerInstance.GetComponent<IHexOccupant<Hex>>();
            case HexContentType.Enemy:
                var enemyPrefab = occupantPrefab[Random.Range(0, occupantPrefab.Count)];
                var enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity, parent);
                return enemyInstance.GetComponent<IHexOccupant<Hex>>();
            case HexContentType.Boss:
                var bossPrefab = occupantPrefab[Random.Range(0, occupantPrefab.Count)];
                var bossInstance = Instantiate(bossPrefab, position, Quaternion.identity, parent);
                return bossInstance.GetComponent<IHexOccupant<Hex>>();
            default:
                return null;
        }
    }
}

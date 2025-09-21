using CardSystem;
using UnityEngine;

public class GameInstance : MonoBehaviour
{

    public CardData[] cardsData;

    public EnemyData[] enemyData;


    public static GameInstance Singleton;
    private bool isInitialized;

    /// <summary>
    /// Initializes the GameInstance as a singleton.
    /// </summary>
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsInitialized()
    {
        return isInitialized;
    }
}

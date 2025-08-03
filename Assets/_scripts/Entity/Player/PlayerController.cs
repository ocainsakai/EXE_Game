using Ain;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public CardList deckList;
    [SerializeField] public EnemyList enemyList;
    [SerializeField] public PlayerHand handController;
    [SerializeField] private HealthComponent healthController;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    public Health Health { get; private set; }

    private void Awake()
    {
        Health = new Health(healthController);
        playerStateMachine.Initialiez(this);
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        
        // Add other input handling as needed
    }
}
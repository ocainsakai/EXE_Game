using Ain;

public class PlayerDrawPhase : IState
{
    private PlayerHand _playerHand;
    private CardList deck;
    public PlayerDrawPhase(PlayerHand playerHand, CardList deck)
    {
        _playerHand = playerHand;
        this.deck = deck;
    }

    public void OnEnter()
    {
        _playerHand.DrawCard(deck);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}

using Entitas;

public enum PlayerIndex
{
    Player1,
    Player2,
    Player3,
    Player4
}

public class PlayerComponent : IComponent
{
    public PlayerIndex index;
    public PlayerActions actions;
}

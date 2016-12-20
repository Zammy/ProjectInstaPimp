using Entitas;

public enum PlayerIndex
{
    Player1,
    Player2,
    Player3,
    Player4
}

[Input, Objects]
public class PlayerIndexComponent : IComponent
{
    public PlayerIndex value;
}

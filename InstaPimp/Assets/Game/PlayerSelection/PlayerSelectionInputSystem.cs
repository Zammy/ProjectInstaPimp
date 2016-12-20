using Entitas;
using InControl;
using System.Linq;
using System.Collections.Generic;

public class PlayerSelectionInputSystem : ISetPool, IExecuteSystem
{
    Pool _pool;
    Group _players;

    public void SetPool(Pool pool)
    {
        _pool = pool;
        _players = pool.GetGroup(ObjectsMatcher.PlayerActions);
    }

    public void Execute()
    {
        var players = _players.GetEntities();
        InputDevice activeDevice = InputManager.ActiveDevice;

        var activePlayer = players.FirstOrDefault(p => p.playerActions.value.Device == activeDevice);
        int activePlayers = players.Length;

        if (activeDevice.Action1.WasPressed
            && activePlayer == null
            && activePlayers <= 3)
        {
            var playerIndexes = new List<PlayerIndex>((PlayerIndex[])System.Enum.GetValues(typeof(PlayerIndex)));
            var freeIndexes = playerIndexes.Where(playerIndex =>
            {
                return !players.Any(p => p.playerIndex.value == playerIndex);
            });
            var freeIndex = freeIndexes.First();
            var playerActions = PlayerActions.CreateWithDefaultBindings();
            playerActions.Device = activeDevice;
            _pool.CreateEntity()
                .AddPlayerActions(playerActions)
                .AddPlayerIndex(freeIndex);
        }

        if (activeDevice.Action2.WasPressed &&
            activePlayer != null)
        {
            _pool.DestroyEntity(activePlayer);
        }
    }
}

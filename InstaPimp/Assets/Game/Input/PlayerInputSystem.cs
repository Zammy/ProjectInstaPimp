using Entitas;
using InControl;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : ISetPool, IExecuteSystem
{
    Pool _pool;
    Group _players;

    public void SetPool(Pool pool)
    {
        _pool = pool;
        _players = pool.GetGroup(Matcher.Player);
    }

    public void Execute()
    {
        if (_players.count == 0)
        {
            return;
        }
              
        var playersEntities = _players.GetEntities();
        foreach (var player in playersEntities.Select(p => p.player))
        {
            var playerActions = player.actions;
            var moveValue = playerActions.Move.Value;
            if (Mathf.Abs(moveValue) > Mathf.Epsilon)
            {
                _pool.CreateEntity()
                    .AddMovementInput(player.index, moveValue);
            }

            var aimValue = playerActions.Aim.Value;
            if (Mathf.Abs(aimValue.sqrMagnitude) > Mathf.Epsilon)
            {
                _pool.CreateEntity()
                    .AddAimInput(aimValue);
            }

            if (playerActions.Jump.WasPressed)
            {
                _pool.CreateEntity()
                    .AddActionInput(PlayerActionType.Jump);
            }


            if (playerActions.Fire.WasPressed)
            {
                _pool.CreateEntity()
                    .AddActionInput(PlayerActionType.Fire);
            }
        }
    }
}

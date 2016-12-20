using System.Collections.Generic;
using Entitas;
using System.Linq;
using UnityEngine;

public class PlayerMovementSystem : ISetPools, IReactiveSystem
{
    Group _playerViews;

    public void SetPools(Pools pools)
    {
        _playerViews = pools.objects.GetGroup(ObjectsMatcher.View);
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return InputMatcher.MovementInput.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entities)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];
            if (!entity.hasMovementInput
                || !entity.hasPlayerIndex)
            {
                continue;  
            }

            var playerIndex = entity.playerIndex.value;
            var move = entity.movementInput.value;

            var playerController = _playerViews.GetEntities()
                .First(e => e.hasView && e.hasPlayerIndex && e.playerIndex.value == playerIndex)
                .view
                .controller;

            var moveSpeed = Pools.sharedInstance.settings.playerOptions.MoveSpeed;
            var playerVelocity = playerController.Velocity;
            if (Mathf.Abs(move) > 0.15f)
            {
                if (move > 0)
                {
                    playerVelocity.x = moveSpeed;
                }

                if (move < 0)
                {
                    playerVelocity.x = -moveSpeed;
                }
            }
            else
            {
                playerVelocity.x = 0;
            }
            playerController.Velocity = playerVelocity;
        }
    }


}

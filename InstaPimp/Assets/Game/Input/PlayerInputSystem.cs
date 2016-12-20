using Entitas;
using UnityEngine;

public class PlayerInputSystem : ISetPools, IExecuteSystem, ICleanupSystem
{
    Pool _inputPool;
    Group _playersActions;

    public void SetPools(Pools pools)
    {
        _inputPool = pools.input;
        _playersActions = pools.objects.GetGroup(ObjectsMatcher.PlayerActions);
    }

    public void Execute()
    {
        if (_playersActions.count == 0)
        {
            return;
        }

        var playersEntities = _playersActions.GetEntities();
        foreach (var player in playersEntities)
        {
            var playerActions = player.playerActions.value;
            var playerIndex = player.playerIndex.value;
            var moveValue = playerActions.Move.Value;
            if (Mathf.Abs(moveValue) > Mathf.Epsilon)
            {
                _inputPool.CreateEntity()
                    .AddMovementInput(moveValue)
                    .AddPlayerIndex(playerIndex);
            }

            var aimValue = playerActions.Aim.Value;
            if (Mathf.Abs(aimValue.sqrMagnitude) > Mathf.Epsilon)
            {
                //_inputPool.CreateEntity()
                //    .ReplaceAimInput(aimValue);
            }

            if (playerActions.Jump.WasPressed)
            {
                //_inputPool.CreateEntity()
                //    .AddActionInput(PlayerActionType.Jump);
            }

            if (playerActions.Fire.WasPressed)
            {
                //_inputPool.CreateEntity()
                //    .AddActionInput(player.index, PlayerActionType.Fire);
            }
        }
    }

    public void Cleanup()
    {
        var inputEntites = _inputPool.GetEntities();
        for (int i = 0; i < inputEntites.Length; i++)
        {
            _inputPool.DestroyEntity(inputEntites[i]);
        }
    }
}

using Entitas;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionCreateCageSystem : ISetPool, IReactiveSystem
{
    Pool _pool;
    Group _cages;
    Group _players;

    public void SetPool(Pool pool)
    {
        _pool = pool;
        _cages = pool.GetGroup(Matcher.Cage);
        _players = pool.GetGroup(Matcher.Player);
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.Player.OnEntityAddedOrRemoved();
        }
    }

    public void Execute(List<Entity> _)
    {
        var players = _players.GetEntities();
        var cages = _cages.GetEntities();
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i].player;
            var cage = cages.FirstOrDefault<Entity>(c => c.cage.playerIndex == player.index);
            if (cage == null)
            {
                var prefab = Resources.Load<GameObject>(Res.PlayerCage);
                var cageGo = (GameObject)UnityEngine.Object.Instantiate(prefab);
                _pool.CreateEntity()
                    .AddCage(player.index, cageGo);
            }
        }

        for (int i = 0; i < cages.Length; i++)
        {
            var cage = cages[i];
            var player = players.FirstOrDefault<Entity>(p => p.player.index == cage.cage.playerIndex);
            if (player == null)
            {
                UnityEngine.GameObject.Destroy(cage.cage.gameObject);
                _pool.DestroyEntity(cage);
            }
        }
    }
}

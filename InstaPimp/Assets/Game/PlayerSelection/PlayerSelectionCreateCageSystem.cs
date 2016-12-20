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
        _cages = pool.GetGroup(ObjectsMatcher.Cage);
        _players = pool.GetGroup(ObjectsMatcher.PlayerActions);
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return ObjectsMatcher.PlayerActions.OnEntityAddedOrRemoved();
        }
    }

    public void Execute(List<Entity> _)
    {
        var players = _players.GetEntities();
        var cages = _cages.GetEntities();
        for (int i = 0; i < players.Length; i++)
        {
            var playerIndex = players[i].playerIndex.value;
            var cageEnt = cages.FirstOrDefault<Entity>(c => c.playerIndex.value == playerIndex);
            if (cageEnt == null)
            {
                var prefab = Resources.Load<GameObject>(Res.PlayerCage);
                var cageGo = (GameObject)UnityEngine.Object.Instantiate(prefab);

                prefab = Resources.Load<GameObject>(Res.Player);
                var playerGo = (GameObject)UnityEngine.Object.Instantiate(prefab);

                cageEnt = _pool.CreateEntity()
                            .AddCage(cageGo, playerGo)
                            .AddPlayerIndex(playerIndex);
                cageGo.Link(cageEnt, _pool);

                var viewController = playerGo.GetComponent<ViewController>();
                var playerEnt = _pool.CreateEntity()
                            .AddView(viewController)
                            .AddPlayerIndex(playerIndex);
                playerGo.Link(playerEnt, _pool);
            }
        }

        for (int i = 0; i < cages.Length; i++)
        {
            var cage = cages[i];
            var player = players.FirstOrDefault<Entity>(p => p.playerIndex.value == cage.playerIndex.value);
            if (player == null)
            {
                UnityEngine.GameObject.Destroy(cage.cage.cageGo);
                UnityEngine.GameObject.Destroy(cage.cage.playerGo);
                _pool.DestroyEntity(cage);
            }
        }
    }
}


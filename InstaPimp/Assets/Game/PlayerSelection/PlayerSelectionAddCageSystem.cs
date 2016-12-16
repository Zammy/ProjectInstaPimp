using Entitas;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionAddCageSystem : ISetPool, IReactiveSystem
{
    Pool _pool;
    Group _cages;

    public void SetPool(Pool pool)
    {
        _pool = pool;
        _cages = pool.GetGroup(Matcher.Cage);
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.Player.OnEntityAdded();
        }
    }

    public void Execute(List<Entity> entites)
    {
        foreach (var e in entites)
        {
            int cages = _cages.count;
            var player = e.player;

            //create a player cage
            var prefab = Resources.Load<GameObject>(Res.PlayerCage);
            var cageGo = (GameObject) UnityEngine.Object.Instantiate(prefab);
            _pool.CreateEntity()
                .AddCage(player.index, cageGo);

        }
    }
}

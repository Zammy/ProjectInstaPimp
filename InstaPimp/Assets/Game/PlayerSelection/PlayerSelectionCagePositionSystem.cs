using Entitas;
using System.Collections.Generic;

public class PlayerSelectionCagePositionSystem : ISetPool, IReactiveSystem 
{
    readonly float[][] _positions =
    {
        new float[] { 0 },
        new float[] { -10, 10 },
        new float[] { -10, 0, 10 },
        new float[] { -15, -5, 5, 15 },
    };

    Group _cages;

    public void SetPool(Pool pool)
    {
        _cages = pool.GetGroup(ObjectsMatcher.Cage);
    }

    public TriggerOnEvent trigger
    {
        get
        {
            return ObjectsMatcher.Cage.OnEntityAddedOrRemoved();
        }
    }

    public void Execute(List<Entity> _)
    {
        var cages = _cages.GetEntities();
        for (int i = 0; i < cages.Length; i++)
        {
            var cage = cages[i].cage;
            var go = cage.cageGo;
            float x = _positions[cages.Length - 1][i];
            go.transform.position = new UnityEngine.Vector3(x, 0, 0);
            go = cage.playerGo;
            go.transform.position = new UnityEngine.Vector3(x, 0, 0);
        }
    }
}

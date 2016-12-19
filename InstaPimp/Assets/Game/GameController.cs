using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    Systems _systems;

    void Start() 
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        _systems = createSystems(pools.pool);
        _systems.Initialize();
    }

    void Update() 
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    void OnDestroy() 
    {
        _systems.TearDown();
    }

    Systems createSystems(Pool pool) 
    {
        return new Feature("PlayerSelection")
            .Add(pool.CreateSystem(new PlayerInputSystem()))
            .Add(pool.CreateSystem(new PlayerSelectionInputSystem()))
            .Add(pool.CreateSystem(new PlayerSelectionCagePositionSystem()))
            .Add(pool.CreateSystem(new PlayerSelectionCreateCageSystem()))
            ;
    }
}

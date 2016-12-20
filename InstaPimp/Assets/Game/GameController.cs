using Entitas;
using Entitas.Unity.Serialization.Blueprints;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    public Blueprints blueprints;

    Systems _systems;

    void Start() 
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        pools.settings.CreateEntity().ApplyBlueprint(blueprints.PlayerSettings);

        _systems = createSystems(pools);
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

    Systems createSystems(Pools pool) 
    {
        return new Feature("Systems")
            .Add(pool.input.CreateSystem(new PlayerInputSystem()))
            .Add(pool.objects.CreateSystem(new PlayerSelectionInputSystem()))
            .Add(pool.objects.CreateSystem(new PlayerSelectionCagePositionSystem()))
            .Add(pool.objects.CreateSystem(new PlayerSelectionCreateCageSystem()))
            .Add(pool.input.CreateSystem(new PlayerMovementSystem()))
            ;
    }
}

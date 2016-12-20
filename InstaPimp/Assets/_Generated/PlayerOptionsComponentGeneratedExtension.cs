//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {

    public partial class Entity {

        public PlayerOptionsComponent playerOptions { get { return (PlayerOptionsComponent)GetComponent(SettingsComponentIds.PlayerOptions); } }
        public bool hasPlayerOptions { get { return HasComponent(SettingsComponentIds.PlayerOptions); } }

        public Entity AddPlayerOptions(float newMoveSpeed) {
            var component = CreateComponent<PlayerOptionsComponent>(SettingsComponentIds.PlayerOptions);
            component.MoveSpeed = newMoveSpeed;
            return AddComponent(SettingsComponentIds.PlayerOptions, component);
        }

        public Entity ReplacePlayerOptions(float newMoveSpeed) {
            var component = CreateComponent<PlayerOptionsComponent>(SettingsComponentIds.PlayerOptions);
            component.MoveSpeed = newMoveSpeed;
            ReplaceComponent(SettingsComponentIds.PlayerOptions, component);
            return this;
        }

        public Entity RemovePlayerOptions() {
            return RemoveComponent(SettingsComponentIds.PlayerOptions);
        }
    }

    public partial class Pool {

        public Entity playerOptionsEntity { get { return GetGroup(SettingsMatcher.PlayerOptions).GetSingleEntity(); } }
        public PlayerOptionsComponent playerOptions { get { return playerOptionsEntity.playerOptions; } }
        public bool hasPlayerOptions { get { return playerOptionsEntity != null; } }

        public Entity SetPlayerOptions(float newMoveSpeed) {
            if(hasPlayerOptions) {
                throw new EntitasException("Could not set playerOptions!\n" + this + " already has an entity with PlayerOptionsComponent!",
                    "You should check if the pool already has a playerOptionsEntity before setting it or use pool.ReplacePlayerOptions().");
            }
            var entity = CreateEntity();
            entity.AddPlayerOptions(newMoveSpeed);
            return entity;
        }

        public Entity ReplacePlayerOptions(float newMoveSpeed) {
            var entity = playerOptionsEntity;
            if(entity == null) {
                entity = SetPlayerOptions(newMoveSpeed);
            } else {
                entity.ReplacePlayerOptions(newMoveSpeed);
            }

            return entity;
        }

        public void RemovePlayerOptions() {
            DestroyEntity(playerOptionsEntity);
        }
    }
}

    public partial class SettingsMatcher {

        static IMatcher _matcherPlayerOptions;

        public static IMatcher PlayerOptions {
            get {
                if(_matcherPlayerOptions == null) {
                    var matcher = (Matcher)Matcher.AllOf(SettingsComponentIds.PlayerOptions);
                    matcher.componentNames = SettingsComponentIds.componentNames;
                    _matcherPlayerOptions = matcher;
                }

                return _matcherPlayerOptions;
            }
        }
    }
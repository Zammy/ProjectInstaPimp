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

        public CageComponent cage { get { return (CageComponent)GetComponent(ObjectsComponentIds.Cage); } }
        public bool hasCage { get { return HasComponent(ObjectsComponentIds.Cage); } }

        public Entity AddCage(UnityEngine.GameObject newCageGo, UnityEngine.GameObject newPlayerGo) {
            var component = CreateComponent<CageComponent>(ObjectsComponentIds.Cage);
            component.cageGo = newCageGo;
            component.playerGo = newPlayerGo;
            return AddComponent(ObjectsComponentIds.Cage, component);
        }

        public Entity ReplaceCage(UnityEngine.GameObject newCageGo, UnityEngine.GameObject newPlayerGo) {
            var component = CreateComponent<CageComponent>(ObjectsComponentIds.Cage);
            component.cageGo = newCageGo;
            component.playerGo = newPlayerGo;
            ReplaceComponent(ObjectsComponentIds.Cage, component);
            return this;
        }

        public Entity RemoveCage() {
            return RemoveComponent(ObjectsComponentIds.Cage);
        }
    }
}

    public partial class ObjectsMatcher {

        static IMatcher _matcherCage;

        public static IMatcher Cage {
            get {
                if(_matcherCage == null) {
                    var matcher = (Matcher)Matcher.AllOf(ObjectsComponentIds.Cage);
                    matcher.componentNames = ObjectsComponentIds.componentNames;
                    _matcherCage = matcher;
                }

                return _matcherCage;
            }
        }
    }

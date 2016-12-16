//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Entitas {

    public partial class Entity {

        public AimInputComponent aimInput { get { return (AimInputComponent)GetComponent(ComponentIds.AimInput); } }
        public bool hasAimInput { get { return HasComponent(ComponentIds.AimInput); } }

        public Entity AddAimInput(UnityEngine.Vector2 newValue) {
            var component = CreateComponent<AimInputComponent>(ComponentIds.AimInput);
            component.value = newValue;
            return AddComponent(ComponentIds.AimInput, component);
        }

        public Entity ReplaceAimInput(UnityEngine.Vector2 newValue) {
            var component = CreateComponent<AimInputComponent>(ComponentIds.AimInput);
            component.value = newValue;
            ReplaceComponent(ComponentIds.AimInput, component);
            return this;
        }

        public Entity RemoveAimInput() {
            return RemoveComponent(ComponentIds.AimInput);
        }
    }

    public partial class Matcher {

        static IMatcher _matcherAimInput;

        public static IMatcher AimInput {
            get {
                if(_matcherAimInput == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.AimInput);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherAimInput = matcher;
                }

                return _matcherAimInput;
            }
        }
    }
}

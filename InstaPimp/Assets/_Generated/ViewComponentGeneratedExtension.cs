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

        public ViewComponent view { get { return (ViewComponent)GetComponent(ObjectsComponentIds.View); } }
        public bool hasView { get { return HasComponent(ObjectsComponentIds.View); } }

        public Entity AddView(IViewController newController) {
            var component = CreateComponent<ViewComponent>(ObjectsComponentIds.View);
            component.controller = newController;
            return AddComponent(ObjectsComponentIds.View, component);
        }

        public Entity ReplaceView(IViewController newController) {
            var component = CreateComponent<ViewComponent>(ObjectsComponentIds.View);
            component.controller = newController;
            ReplaceComponent(ObjectsComponentIds.View, component);
            return this;
        }

        public Entity RemoveView() {
            return RemoveComponent(ObjectsComponentIds.View);
        }
    }
}

    public partial class ObjectsMatcher {

        static IMatcher _matcherView;

        public static IMatcher View {
            get {
                if(_matcherView == null) {
                    var matcher = (Matcher)Matcher.AllOf(ObjectsComponentIds.View);
                    matcher.componentNames = ObjectsComponentIds.componentNames;
                    _matcherView = matcher;
                }

                return _matcherView;
            }
        }
    }
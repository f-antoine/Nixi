using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.AbstractMapping
{
    public sealed class ExamplesWithAbstractComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public AbstractComponentBase InstanceTransposed;

        [NixInjectComponentFromChildren]
        public AbstractComponentBase InstanceTransposedChildren;

        [NixInjectComponentFromChildren("Children")]
        public AbstractComponentBase InstanceTransposedChildrenWithName;

        [NixInjectComponentFromParent]
        public AbstractComponentBase InstanceTransposedParent;

        [NixInjectComponentFromParent("Parent")]
        public AbstractComponentBase InstanceTransposedParentWithName;
        
        [NixInjectRootComponent("Root")]
        public AbstractComponentBase RootInstanceTransposed;

        [NixInjectRootComponent("Root", "Children")]
        public AbstractComponentBase RootChildrenInstanceTransposed;
    }
}
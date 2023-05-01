using Nixi.Injections;

namespace ScriptExample.AbstractMapping
{
    public sealed class ExamplesWithAbstractComponent : MonoBehaviourInjectable
    {
        [Component]
        public AbstractComponentBase InstanceTransposed;

        [ComponentFromChildren]
        public AbstractComponentBase InstanceTransposedChildren;

        [ComponentFromChildren("Children")]
        public AbstractComponentBase InstanceTransposedChildrenWithName;

        [ComponentFromParents]
        public AbstractComponentBase InstanceTransposedParent;

        [ComponentFromParents("Parent")]
        public AbstractComponentBase InstanceTransposedParentWithName;
        
        [RootComponent("Root")]
        public AbstractComponentBase RootInstanceTransposed;

        [RootComponent("Root", "Children")]
        public AbstractComponentBase RootChildrenInstanceTransposed;
    }
}
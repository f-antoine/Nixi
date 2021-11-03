using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Fallen.List
{
    public sealed class FallenCompoListClass : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public EmptyClass FallenEmptyClass;
    }
}
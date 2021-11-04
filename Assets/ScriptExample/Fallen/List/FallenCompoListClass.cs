using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Fallen.List
{
    public sealed class FallenCompoListClass : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public EmptyClass FallenEmptyClass;
    }
}
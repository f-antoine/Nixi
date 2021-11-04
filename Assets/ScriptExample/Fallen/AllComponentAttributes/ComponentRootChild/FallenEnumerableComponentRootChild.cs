using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any", "anyChild")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
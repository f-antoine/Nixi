using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
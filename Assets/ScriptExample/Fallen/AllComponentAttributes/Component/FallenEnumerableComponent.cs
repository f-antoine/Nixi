using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
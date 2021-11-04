using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
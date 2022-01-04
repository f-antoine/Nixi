using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenEnumerableComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
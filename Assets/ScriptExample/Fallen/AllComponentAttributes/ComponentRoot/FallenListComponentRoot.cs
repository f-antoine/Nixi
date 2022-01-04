using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public List<EmptyClass> FallenElement;
    }
}
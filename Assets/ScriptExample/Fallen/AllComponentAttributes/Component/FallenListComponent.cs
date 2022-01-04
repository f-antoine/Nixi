using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public List<EmptyClass> FallenElement;
    }
}
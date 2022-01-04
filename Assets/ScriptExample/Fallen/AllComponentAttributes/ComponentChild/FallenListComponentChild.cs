using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public List<EmptyClass> FallenElement;
    }
}
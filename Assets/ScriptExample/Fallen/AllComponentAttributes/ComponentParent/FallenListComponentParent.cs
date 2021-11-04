using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public List<EmptyClass> FallenElement;
    }
}
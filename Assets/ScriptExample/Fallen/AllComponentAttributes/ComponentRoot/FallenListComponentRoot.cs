using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public List<EmptyClass> FallenElement;
    }
}
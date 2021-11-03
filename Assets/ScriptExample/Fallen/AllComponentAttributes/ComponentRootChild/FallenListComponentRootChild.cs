using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponentRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any", "anyChild")]
        public List<EmptyClass> FallenElement;
    }
}
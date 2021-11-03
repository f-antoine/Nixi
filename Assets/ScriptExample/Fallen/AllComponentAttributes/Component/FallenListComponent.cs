using Nixi.Injections;
using Nixi.Injections.Attributes;
using System.Collections.Generic;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenListComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public List<EmptyClass> FallenElement;
    }
}
using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRoot
{
    public sealed class FallenListComponentRoot : MonoBehaviourInjectable
    {
        [RootComponent("any")]
        public List<EmptyClass> FallenElement;
    }
}
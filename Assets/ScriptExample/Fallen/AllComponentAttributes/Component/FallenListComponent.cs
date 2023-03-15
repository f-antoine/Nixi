using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenListComponent : MonoBehaviourInjectable
    {
        [Component]
        public List<EmptyClass> FallenElement;
    }
}
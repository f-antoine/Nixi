using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenEnumerableComponent : MonoBehaviourInjectable
    {
        [Component]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
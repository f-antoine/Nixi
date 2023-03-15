using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenEnumerableComponentChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
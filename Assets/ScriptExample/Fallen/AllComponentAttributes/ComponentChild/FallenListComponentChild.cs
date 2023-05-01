using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenListComponentChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("any")]
        public List<EmptyClass> FallenElement;
    }
}
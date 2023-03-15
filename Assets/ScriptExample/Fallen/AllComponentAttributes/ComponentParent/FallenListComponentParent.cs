using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentParent
{
    public sealed class FallenListComponentParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("any")]
        public List<EmptyClass> FallenElement;
    }
}
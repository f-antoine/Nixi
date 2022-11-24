using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenListComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public List<EmptyClass> FallenElement;
    }
}
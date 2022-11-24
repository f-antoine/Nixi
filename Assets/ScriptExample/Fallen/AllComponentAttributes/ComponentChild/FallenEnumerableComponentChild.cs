using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenEnumerableComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
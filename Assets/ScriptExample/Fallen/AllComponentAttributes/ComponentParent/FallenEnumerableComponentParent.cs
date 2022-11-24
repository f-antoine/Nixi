using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentParent
{
    public sealed class FallenEnumerableComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRoot
{
    public sealed class FallenEnumerableComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenEnumerableComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
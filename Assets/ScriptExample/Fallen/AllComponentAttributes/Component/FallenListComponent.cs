using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenListComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public List<EmptyClass> FallenElement;
    }
}
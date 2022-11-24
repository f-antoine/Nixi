using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenArrayComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public EmptyClass[] FallenElement;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRoot
{
    public sealed class FallenArrayComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public EmptyClass[] FallenElement;
    }
}
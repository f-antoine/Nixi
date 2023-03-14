using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRoot
{
    public sealed class FallenArrayComponentRoot : MonoBehaviourInjectable
    {
        [RootComponent("any")]
        public EmptyClass[] FallenElement;
    }
}
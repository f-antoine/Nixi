using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenArrayComponentChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("any")]
        public EmptyClass[] FallenElement;
    }
}
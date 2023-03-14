using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentParent
{
    public sealed class FallenArrayComponentParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("any")]
        public EmptyClass[] FallenElement;
    }
}
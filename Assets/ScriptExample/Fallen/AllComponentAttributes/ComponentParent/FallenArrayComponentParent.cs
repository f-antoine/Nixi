using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentParent
{
    public sealed class FallenArrayComponentParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("any")]
        public EmptyClass[] FallenElement;
    }
}
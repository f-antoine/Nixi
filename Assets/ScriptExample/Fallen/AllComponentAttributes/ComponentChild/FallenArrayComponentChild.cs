using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentChild
{
    public sealed class FallenArrayComponentChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("any")]
        public EmptyClass[] FallenElement;
    }
}
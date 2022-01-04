using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any", "anyChild")]
        public EmptyClass[] FallenElement;
    }
}
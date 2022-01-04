using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public EmptyClass[] FallenElement;
    }
}
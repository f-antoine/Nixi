using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any")]
        public EmptyClass[] FallenElement;
    }
}
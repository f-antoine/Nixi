using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("any", "anyChild")]
        public EmptyClass[] FallenElement;
    }
}
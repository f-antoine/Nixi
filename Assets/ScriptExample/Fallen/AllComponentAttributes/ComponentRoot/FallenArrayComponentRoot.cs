using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRoot
{
    public sealed class FallenArrayComponentRoot : MonoBehaviourInjectable
    {
        [RootComponent("any")]
        public EmptyClass[] FallenElement;
    }
}
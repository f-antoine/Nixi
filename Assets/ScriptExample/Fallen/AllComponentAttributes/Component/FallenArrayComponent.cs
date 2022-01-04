using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public EmptyClass[] FallenElement;
    }
}
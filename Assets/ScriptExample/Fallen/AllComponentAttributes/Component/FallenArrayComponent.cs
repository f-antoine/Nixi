using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public EmptyClass[] FallenElement;
    }
}
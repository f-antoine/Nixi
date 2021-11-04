using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public EmptyClass[] FallenElement;
    }
}
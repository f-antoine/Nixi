using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        public EmptyClass[] FallenElement;
    }
}
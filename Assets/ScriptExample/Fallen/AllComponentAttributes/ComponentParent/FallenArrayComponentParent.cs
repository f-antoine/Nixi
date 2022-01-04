using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public EmptyClass[] FallenElement;
    }
}
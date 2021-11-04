using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("any")]
        public EmptyClass[] FallenElement;
    }
}
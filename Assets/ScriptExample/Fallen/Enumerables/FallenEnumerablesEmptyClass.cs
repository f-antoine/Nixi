using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesEmptyClass : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public EmptyClass Fallen;
    }
}
using Nixi.Injections;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesEmptyClass : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public EmptyClass Fallen;
    }
}
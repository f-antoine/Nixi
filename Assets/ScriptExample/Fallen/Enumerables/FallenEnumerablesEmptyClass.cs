using Nixi.Injections;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesEmptyClass : MonoBehaviourInjectable
    {
        [Components]
        public EmptyClass Fallen;
    }
}
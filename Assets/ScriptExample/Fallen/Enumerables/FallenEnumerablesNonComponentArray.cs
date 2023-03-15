using Nixi.Injections;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentArray : MonoBehaviourInjectable
    {
        [Components]
        public int[] Fallen;
    }
}
using Nixi.Injections;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentArray : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public int[] Fallen;
    }
}
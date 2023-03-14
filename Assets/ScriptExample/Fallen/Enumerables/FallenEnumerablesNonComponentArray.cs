using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentArray : MonoBehaviourInjectable
    {
        [Components]
        public int[] Fallen;
    }
}
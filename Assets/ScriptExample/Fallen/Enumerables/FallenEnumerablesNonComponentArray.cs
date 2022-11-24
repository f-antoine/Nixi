using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentArray : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public int[] Fallen;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using System.Collections.Generic;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentEnumerable : MonoBehaviourInjectable
    {
        [Components]
        public IEnumerable<int> Fallen;
    }
}
using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesNonComponentList : MonoBehaviourInjectable
    {
        [Components]
        public List<int> Fallen;
    }
}
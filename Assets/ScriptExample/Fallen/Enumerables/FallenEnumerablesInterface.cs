using Nixi.Injections;
using ScriptExample.Containers;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesInterface : MonoBehaviourInjectable
    {
        [Components]
        public ITestInterface Fallen;
    }
}
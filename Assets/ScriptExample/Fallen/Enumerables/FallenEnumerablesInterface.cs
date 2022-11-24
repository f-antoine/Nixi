using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using ScriptExample.Containers;

namespace ScriptExample.Fallen.Enumerables
{
    public sealed class FallenEnumerablesInterface : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public ITestInterface Fallen;
    }
}
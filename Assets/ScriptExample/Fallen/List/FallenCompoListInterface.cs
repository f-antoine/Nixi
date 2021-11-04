using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Containers;

namespace ScriptExample.Fallen.List
{
    public sealed class FallenCompoListInterface : MonoBehaviourInjectable
    {
        [NixInjectComponents]
        public ITestInterface FallenInterface;
    }
}
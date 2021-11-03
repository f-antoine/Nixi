using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Containers;

namespace Assets.ScriptExample.Fallen.List
{
    public sealed class FallenCompoListInterface : MonoBehaviourInjectable
    {
        [NixInjectComponentList]
        public ITestInterface FallenInterface;
    }
}
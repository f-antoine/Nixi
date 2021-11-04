using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName", "rootChild")]
        public IGenericInterface<int> RootChildGenericityInterface;
    }
}
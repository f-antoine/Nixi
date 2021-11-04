using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName")]
        public IGenericInterface<int> RootGenericityInterface;
    }
}
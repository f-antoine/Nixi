using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public IGenericInterface<int> RootChildGenericityInterface;
    }
}
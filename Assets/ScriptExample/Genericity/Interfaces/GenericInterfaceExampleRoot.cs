using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public IGenericInterface<int> RootGenericityInterface;
    }
}
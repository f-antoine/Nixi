using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IGenericInterface<int> GenericityInterface;
    }
}

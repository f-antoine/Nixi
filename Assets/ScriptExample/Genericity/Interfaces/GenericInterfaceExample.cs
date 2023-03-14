using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExample : MonoBehaviourInjectable
    {
        [Component]
        public IGenericInterface<int> GenericityInterface;
    }
}

using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildName")]
        public IGenericInterface<int> ChildGenericityInterface;
    }
}

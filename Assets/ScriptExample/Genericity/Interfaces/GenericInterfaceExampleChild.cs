using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public IGenericInterface<int> ChildGenericityInterface;
    }
}

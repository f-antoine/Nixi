using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public IMultipleGenericInterface<int, string> ChildGenericityInterface;
    }
}

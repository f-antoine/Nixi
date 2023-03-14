using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExample : MonoBehaviourInjectable
    {
        [Component]
        public IMultipleGenericInterface<int, string> GenericityInterface;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
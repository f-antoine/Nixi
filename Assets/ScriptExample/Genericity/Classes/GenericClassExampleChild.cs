using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public GenericClass<int> GenericClass;
    }
}
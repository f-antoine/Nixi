using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
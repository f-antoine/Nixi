using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
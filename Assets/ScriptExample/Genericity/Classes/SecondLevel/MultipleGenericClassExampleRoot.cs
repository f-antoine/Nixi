using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
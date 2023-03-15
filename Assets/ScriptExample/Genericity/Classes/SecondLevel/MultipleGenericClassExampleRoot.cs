using Nixi.Injections;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
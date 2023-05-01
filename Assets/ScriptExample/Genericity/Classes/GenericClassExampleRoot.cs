using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public GenericClass<int> GenericClass;
    }
}
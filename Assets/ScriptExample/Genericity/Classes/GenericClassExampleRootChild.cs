using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public GenericClass<int> GenericClass;
    }
}
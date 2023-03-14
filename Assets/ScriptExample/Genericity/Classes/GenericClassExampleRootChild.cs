using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public GenericClass<int> GenericClass;
    }
}
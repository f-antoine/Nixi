using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName", "rootChild")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
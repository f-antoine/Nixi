using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName", "rootChild")]
        public GenericClass<int> GenericClass;
    }
}
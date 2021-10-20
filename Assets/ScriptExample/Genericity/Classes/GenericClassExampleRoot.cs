using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleRoot : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName")]
        public GenericClass<int> GenericClass;
    }
}
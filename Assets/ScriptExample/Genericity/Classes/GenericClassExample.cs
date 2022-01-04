using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public GenericClass<int> GenericClass;
    }
}
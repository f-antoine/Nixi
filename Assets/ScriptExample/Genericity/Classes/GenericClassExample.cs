using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExample : MonoBehaviourInjectable
    {
        [Component]
        public GenericClass<int> GenericClass;
    }
}
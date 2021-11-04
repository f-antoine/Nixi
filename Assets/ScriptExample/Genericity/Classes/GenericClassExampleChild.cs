using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildName")]
        public GenericClass<int> GenericClass;
    }
}
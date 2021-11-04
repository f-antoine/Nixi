using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentName")]
        public GenericClass<int> GenericClass;
    }
}
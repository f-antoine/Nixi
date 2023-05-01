using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public GenericClass<int> GenericClass;
    }
}
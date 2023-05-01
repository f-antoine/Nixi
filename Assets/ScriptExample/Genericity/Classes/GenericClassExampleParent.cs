using Nixi.Injections;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentName")]
        public GenericClass<int> GenericClass;
    }
}
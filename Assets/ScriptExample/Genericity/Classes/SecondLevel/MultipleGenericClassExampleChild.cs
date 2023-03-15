using Nixi.Injections;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleChild : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
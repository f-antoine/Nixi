using Nixi.Injections;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExample : MonoBehaviourInjectable
    {
        [Component]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
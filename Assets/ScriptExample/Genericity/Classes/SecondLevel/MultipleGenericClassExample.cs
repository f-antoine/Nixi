using Nixi.Injections;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
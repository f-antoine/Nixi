using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
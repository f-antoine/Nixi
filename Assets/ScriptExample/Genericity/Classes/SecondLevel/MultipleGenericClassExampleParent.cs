using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentName")]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
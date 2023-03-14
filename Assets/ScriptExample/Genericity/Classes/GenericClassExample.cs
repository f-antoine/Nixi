using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExample : MonoBehaviourInjectable
    {
        [Component]
        public GenericClass<int> GenericClass;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentName")]
        public GenericClass<int> GenericClass;
    }
}
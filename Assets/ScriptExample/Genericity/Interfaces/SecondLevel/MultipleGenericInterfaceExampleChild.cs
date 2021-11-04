using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildName")]
        public IMultipleGenericInterface<int, string> ChildGenericityInterface;
    }
}

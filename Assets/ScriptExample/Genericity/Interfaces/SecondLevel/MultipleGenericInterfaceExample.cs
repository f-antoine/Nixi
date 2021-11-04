using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IMultipleGenericInterface<int, string> GenericityInterface;
    }
}
using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExample : MonoBehaviourInjectable
    {
        [Component]
        public IMultipleGenericInterface<int, string> GenericityInterface;
    }
}
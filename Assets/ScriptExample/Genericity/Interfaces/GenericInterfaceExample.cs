using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExample : MonoBehaviourInjectable
    {
        [Component]
        public IGenericInterface<int> GenericityInterface;
    }
}

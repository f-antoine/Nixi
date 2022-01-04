using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public IGenericInterface<int> GenericityInterface;
    }
}

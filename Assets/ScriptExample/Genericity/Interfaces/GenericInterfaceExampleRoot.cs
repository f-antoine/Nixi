using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public IGenericInterface<int> RootGenericityInterface;
    }
}
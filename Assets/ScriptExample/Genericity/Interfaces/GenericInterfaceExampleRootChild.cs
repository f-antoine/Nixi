using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public IGenericInterface<int> RootChildGenericityInterface;
    }
}
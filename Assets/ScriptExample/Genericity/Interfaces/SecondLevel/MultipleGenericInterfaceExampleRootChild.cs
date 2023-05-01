using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleRootChild : MonoBehaviourInjectable
    {
        [RootComponent("rootName", "rootChild")]
        public IMultipleGenericInterface<int, string> RootChildGenericityInterface;
    }
}
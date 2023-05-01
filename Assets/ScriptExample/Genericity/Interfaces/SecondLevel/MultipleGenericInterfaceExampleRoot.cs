using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleRoot : MonoBehaviourInjectable
    {
        [RootComponent("rootName")]
        public IMultipleGenericInterface<int, string> RootGenericityInterface;
    }
}
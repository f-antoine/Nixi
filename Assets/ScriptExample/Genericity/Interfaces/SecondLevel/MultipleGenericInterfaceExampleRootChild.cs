using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleRootChild : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("rootName", "rootChild")]
        public IMultipleGenericInterface<int, string> RootChildGenericityInterface;
    }
}
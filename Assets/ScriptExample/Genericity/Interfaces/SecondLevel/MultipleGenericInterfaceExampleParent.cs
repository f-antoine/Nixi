using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentName")]
        public IMultipleGenericInterface<int, string> ParentGenericityInterface;
    }
}
using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentName")]
        public IMultipleGenericInterface<int, string> ParentGenericityInterface;
    }
}
using Nixi.Injections;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentName")]
        public IGenericInterface<int> ParentGenericityInterface;
    }
}
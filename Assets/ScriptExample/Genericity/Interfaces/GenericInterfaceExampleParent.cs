using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [ComponentFromParents("ParentName")]
        public IGenericInterface<int> ParentGenericityInterface;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromParent("ParentName")]
        public IGenericInterface<int> ParentGenericityInterface;
    }
}
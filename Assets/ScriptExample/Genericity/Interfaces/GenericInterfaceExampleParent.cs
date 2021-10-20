using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ParentName", GameObjectMethod.GetComponentsInParent)]
        public IGenericInterface<int> ParentGenericityInterface;
    }
}
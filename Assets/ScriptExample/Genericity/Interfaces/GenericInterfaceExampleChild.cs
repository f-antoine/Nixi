using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Interfaces
{
    public sealed class GenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildName", GameObjectMethod.GetComponentsInChildren)]
        public IGenericInterface<int> ChildGenericityInterface;
    }
}

using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildName", GameObjectMethod.GetComponentsInChildren)]
        public IMultipleGenericInterface<int, string> ChildGenericityInterface;
    }
}

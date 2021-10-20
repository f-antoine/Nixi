using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Interfaces.SecondLevel
{
    public sealed class MultipleGenericInterfaceExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ParentName", GameObjectMethod.GetComponentsInParent)]
        public IMultipleGenericInterface<int, string> ParentGenericityInterface;
    }
}
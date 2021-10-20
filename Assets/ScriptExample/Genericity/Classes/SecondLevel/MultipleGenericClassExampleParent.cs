using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ParentName", GameObjectMethod.GetComponentsInParent)]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
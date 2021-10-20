using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes.SecondLevel
{
    public sealed class MultipleGenericClassExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildName", GameObjectMethod.GetComponentsInChildren)]
        public MultipleGenericClass<int, string> GenericClass;
    }
}
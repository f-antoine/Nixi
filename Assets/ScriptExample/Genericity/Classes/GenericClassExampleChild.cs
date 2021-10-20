using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildName", GameObjectMethod.GetComponentsInChildren)]
        public GenericClass<int> GenericClass;
    }
}
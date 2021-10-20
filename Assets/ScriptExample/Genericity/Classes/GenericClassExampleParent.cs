using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExampleParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ParentName", GameObjectMethod.GetComponentsInParent)]
        public GenericClass<int> GenericClass;
    }
}
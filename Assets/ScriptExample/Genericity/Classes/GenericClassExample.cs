using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Genericity.Classes
{
    public sealed class GenericClassExample : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public GenericClass<int> GenericClass;
    }
}
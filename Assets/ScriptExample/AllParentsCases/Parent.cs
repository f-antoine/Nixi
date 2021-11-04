using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.AllParentsCases
{
    public sealed class Parent : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Child FirstChild;

        [NixInjectComponent]
        public Child SecondChild;
    }
}

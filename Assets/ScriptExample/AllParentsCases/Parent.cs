using Nixi.Injections;

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

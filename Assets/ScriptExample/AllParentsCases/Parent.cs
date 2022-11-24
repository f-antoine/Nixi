using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

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

using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.AllParentsCases
{
    public sealed class Parent : MonoBehaviourInjectable
    {
        [Component]
        public Child FirstChild;

        [Component]
        public Child SecondChild;
    }
}

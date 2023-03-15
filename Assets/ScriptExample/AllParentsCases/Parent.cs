using Nixi.Injections;

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

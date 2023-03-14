using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRootChild
{
    public sealed class FallenListComponentRootChild : MonoBehaviourInjectable
    {
        [RootComponent("any", "anyChild")]
        public List<EmptyClass> FallenElement;
    }
}
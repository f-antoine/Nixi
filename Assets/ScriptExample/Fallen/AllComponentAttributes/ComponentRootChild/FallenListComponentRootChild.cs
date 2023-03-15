using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRootChild
{
    public sealed class FallenListComponentRootChild : MonoBehaviourInjectable
    {
        [RootComponent("any", "anyChild")]
        public List<EmptyClass> FallenElement;
    }
}
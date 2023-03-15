using Nixi.Injections;
using System.Collections.Generic;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRootChild
{
    public sealed class FallenEnumerableComponentRootChild : MonoBehaviourInjectable
    {
        [RootComponent("any", "anyChild")]
        public IEnumerable<EmptyClass> FallenElement;
    }
}
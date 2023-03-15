using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes.ComponentRootChild
{
    public sealed class FallenArrayComponentRootChild : MonoBehaviourInjectable
    {
        [RootComponent("any", "anyChild")]
        public EmptyClass[] FallenElement;
    }
}
using Nixi.Injections;

namespace ScriptExample.Fallen.AllComponentAttributes.Component
{
    public sealed class FallenArrayComponent : MonoBehaviourInjectable
    {
        [Component]
        public EmptyClass[] FallenElement;
    }
}
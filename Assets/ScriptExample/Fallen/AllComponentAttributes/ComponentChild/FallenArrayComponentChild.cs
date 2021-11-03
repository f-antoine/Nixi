using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentChild : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public EmptyClass[] FallenElement;
    }
}
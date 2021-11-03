using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace Assets.ScriptExample.Fallen.AllComponentAttributes
{
    public sealed class FallenArrayComponentParent : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInParent)]
        public EmptyClass[] FallenElement;
    }
}
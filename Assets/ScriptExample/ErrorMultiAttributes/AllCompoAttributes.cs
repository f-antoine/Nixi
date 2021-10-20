using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class AllCompoAttributes : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
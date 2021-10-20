using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoFromMethodWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
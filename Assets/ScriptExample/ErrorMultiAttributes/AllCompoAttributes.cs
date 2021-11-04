using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class AllCompoAttributes : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
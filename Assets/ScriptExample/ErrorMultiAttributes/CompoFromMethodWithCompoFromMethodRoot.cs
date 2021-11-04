using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoFromMethodWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("any")]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
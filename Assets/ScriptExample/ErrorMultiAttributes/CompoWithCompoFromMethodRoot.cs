using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoList : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponents]
        public Sorcerer Sorcerer;
    }
}
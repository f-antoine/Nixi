using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoList : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponentList]
        public Sorcerer Sorcerer;
    }
}
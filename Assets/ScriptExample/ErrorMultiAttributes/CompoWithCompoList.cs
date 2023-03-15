using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoList : MonoBehaviourInjectable
    {
        [Component]
        [Components]
        public Sorcerer Sorcerer;
    }
}
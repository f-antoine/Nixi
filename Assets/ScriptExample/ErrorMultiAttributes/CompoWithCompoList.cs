using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.MultiComponents;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
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
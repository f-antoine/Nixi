using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [Component]
        [RootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoFromMethodWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [ComponentFromChildren("any")]
        [RootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
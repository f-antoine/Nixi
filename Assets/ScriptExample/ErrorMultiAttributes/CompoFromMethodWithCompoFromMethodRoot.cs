using Nixi.Injections;
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
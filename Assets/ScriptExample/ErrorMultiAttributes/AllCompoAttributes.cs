using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class AllCompoAttributes : MonoBehaviourInjectable
    {
        [Component]
        [RootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
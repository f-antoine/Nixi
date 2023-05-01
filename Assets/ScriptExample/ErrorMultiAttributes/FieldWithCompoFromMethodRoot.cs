using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [FromContainer]
        [RootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
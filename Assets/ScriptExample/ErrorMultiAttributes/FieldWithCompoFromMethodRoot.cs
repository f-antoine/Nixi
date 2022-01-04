using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
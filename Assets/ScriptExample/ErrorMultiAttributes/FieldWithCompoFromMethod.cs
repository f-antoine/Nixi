using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithCompoFromMethod : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectComponentFromChildren("any")]
        public Sorcerer Sorcerer;
    }
}
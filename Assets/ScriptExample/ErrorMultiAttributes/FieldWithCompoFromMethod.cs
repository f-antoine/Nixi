using Nixi.Injections;
using Nixi.Injections.Attributes;
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
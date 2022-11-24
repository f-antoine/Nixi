using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using Nixi.Injections.Attributes.Fields;
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
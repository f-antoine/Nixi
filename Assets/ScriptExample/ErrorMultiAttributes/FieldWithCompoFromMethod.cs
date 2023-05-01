using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithCompoFromMethod : MonoBehaviourInjectable
    {
        [FromContainer]
        [ComponentFromChildren("any")]
        public Sorcerer Sorcerer;
    }
}
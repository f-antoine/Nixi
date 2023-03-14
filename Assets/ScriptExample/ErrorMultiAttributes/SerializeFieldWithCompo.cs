using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class SerializeFieldWithCompo : MonoBehaviourInjectable
    {
        [Component]
        public Sorcerer Sorcerer;
    }
}
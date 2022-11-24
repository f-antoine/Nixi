using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithSerializeField : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public Sorcerer Sorcerer;
    }
}

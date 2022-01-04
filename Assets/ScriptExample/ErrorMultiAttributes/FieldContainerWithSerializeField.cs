using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithSerializeField : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        public Sorcerer Sorcerer;
    }
}

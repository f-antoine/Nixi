using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithCompo : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectComponent]
        public Sorcerer Sorcerer;
    }
}
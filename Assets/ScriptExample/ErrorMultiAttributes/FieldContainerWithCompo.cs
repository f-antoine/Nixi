using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldContainerWithCompo : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectComponent]
        public Sorcerer Sorcerer;
    }
}
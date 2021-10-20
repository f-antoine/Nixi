using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithMono : MonoBehaviourInjectable
    {
        [NixInject]
        [NixInjectComponent]
        public Sorcerer Sorcerer;
    }
}
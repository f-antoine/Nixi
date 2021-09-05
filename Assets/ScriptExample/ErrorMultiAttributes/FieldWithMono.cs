using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithMono : MonoBehaviourInjectable
    {
        [NixInject]
        [NixInjectMonoBehaviour]
        public Sorcerer Sorcerer;
    }
}
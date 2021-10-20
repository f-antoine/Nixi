using Nixi.Injections;
using Nixi.Injections.Attributes.Fields;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithMonoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInject]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
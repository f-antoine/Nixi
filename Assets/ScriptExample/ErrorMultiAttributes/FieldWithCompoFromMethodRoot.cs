using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class FieldWithCompoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectFromContainer]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
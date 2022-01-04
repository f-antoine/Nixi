using Nixi.Injections;
using ScriptExample.Characters;

namespace ScriptExample.ErrorMultiAttributes
{
    public sealed class SerializeFieldWithCompo : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        public Sorcerer Sorcerer;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class MonoWithMonoFromMethod : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;
    }
}
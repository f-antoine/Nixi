using Nixi.Injections;
using Nixi.Injections.Attributes;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class CompoWithCompoFromMethod : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;
    }
}
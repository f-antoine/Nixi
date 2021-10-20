using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class MonoFromMethodWithMonoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        [NixInjectRootComponent("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
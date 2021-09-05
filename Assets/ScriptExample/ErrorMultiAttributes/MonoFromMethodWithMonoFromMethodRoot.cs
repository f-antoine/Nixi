using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class MonoFromMethodWithMonoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        [NixInjectMonoBehaviourFromMethodRoot("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
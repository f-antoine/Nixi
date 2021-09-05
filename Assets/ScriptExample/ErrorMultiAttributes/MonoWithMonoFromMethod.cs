using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class MonoWithMonoFromMethod : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviour]
        [NixInjectMonoBehaviourFromMethod("any", GameObjectMethod.GetComponentsInChildren)]
        public Sorcerer Sorcerer;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;
using ScriptExample.Characters;

namespace Assets.ScriptExample.ErrorMultiAttributes
{
    public sealed class MonoWithMonoFromMethodRoot : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviour]
        [NixInjectMonoBehaviourFromMethodRoot("anyRoot")]
        public Sorcerer Sorcerer;
    }
}
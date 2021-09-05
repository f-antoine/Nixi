using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcerer : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviour]
        private InfiniteRecursionSorcerer infiniteSorcerer;
    }
}
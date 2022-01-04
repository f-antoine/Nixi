using Nixi.Injections;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcerer : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private InfiniteRecursionSorcerer infiniteSorcerer;
    }
}
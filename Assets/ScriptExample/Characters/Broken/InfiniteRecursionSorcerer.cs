using Nixi.Injections;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcerer : MonoBehaviourInjectable
    {
        [Component]
        private InfiniteRecursionSorcerer infiniteSorcerer;
    }
}
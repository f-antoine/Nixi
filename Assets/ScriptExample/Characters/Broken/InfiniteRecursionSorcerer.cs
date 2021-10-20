using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcerer : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private InfiniteRecursionSorcerer infiniteSorcerer;
    }
}
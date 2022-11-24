using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcerer : MonoBehaviourInjectable
    {
        [NixInjectComponent]
        private InfiniteRecursionSorcerer infiniteSorcerer;
    }
}
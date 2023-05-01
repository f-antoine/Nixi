using Nixi.Injections;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcererWithInheritance : MonoBehaviourInjectable
    {
        [Component]
        private InfiniteRecursionSorcererWithInheritanceSecondLevel infiniteSorcererWithInheritance;

        private class InfiniteRecursionSorcererWithInheritanceSecondLevel : InfiniteRecursionSorcererWithInheritance
        {
        }
    }
}
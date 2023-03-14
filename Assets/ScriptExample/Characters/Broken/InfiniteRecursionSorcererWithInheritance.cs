using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

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
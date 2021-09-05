using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace ScriptExample.Characters.Broken
{
    public class InfiniteRecursionSorcererWithInheritance : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviour]
        private InfiniteRecursionSorcererWithInheritanceSecondLevel infiniteSorcererWithInheritance;

        private class InfiniteRecursionSorcererWithInheritanceSecondLevel : InfiniteRecursionSorcererWithInheritance
        {
        }
    }
}
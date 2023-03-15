using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class RussianDoll : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildDoll")]
        public FirstDoll ChildDoll;

        [ComponentFromChildren("ChildDoll")]
        public FirstDoll ChildDoll2;
    }
}
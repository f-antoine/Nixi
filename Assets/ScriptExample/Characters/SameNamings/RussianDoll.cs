using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class RussianDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildDoll")]
        public FirstDoll ChildDoll;

        [NixInjectComponentFromChildren("ChildDoll")]
        public FirstDoll ChildDoll2;
    }
}
using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildDoll")]
        public SecondDoll ChildDoll;
    }
}
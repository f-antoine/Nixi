using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [ComponentFromChildren("ChildDoll")]
        public SecondDoll ChildDoll;
    }
}
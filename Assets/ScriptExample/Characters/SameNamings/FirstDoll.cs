using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromChildren("ChildDoll")]
        public SecondDoll ChildDoll;
    }
}
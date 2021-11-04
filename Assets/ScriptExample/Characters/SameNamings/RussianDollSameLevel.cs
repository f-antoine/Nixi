using Nixi.Injections;
using Nixi.Injections.Attributes;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevel : MonoBehaviourInjectable
    {
        [NixInjectRootComponent("Doll", "ChildDoll")]
        public FirstDoll Doll;

        [NixInjectRootComponent("Doll", "ChildDoll")]
        public FirstDoll SecondDoll;
    }
}
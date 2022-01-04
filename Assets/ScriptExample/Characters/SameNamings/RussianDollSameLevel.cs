using Nixi.Injections;

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
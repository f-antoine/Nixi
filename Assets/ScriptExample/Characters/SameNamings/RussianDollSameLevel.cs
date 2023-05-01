using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevel : MonoBehaviourInjectable
    {
        [RootComponent("Doll", "ChildDoll")]
        public FirstDoll Doll;

        [RootComponent("Doll", "ChildDoll")]
        public FirstDoll SecondDoll;
    }
}
using Nixi.Injections;

namespace ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevelParent : MonoBehaviourInjectable
    {
        [RootComponent("Doll")]
        public FirstDoll FirstDoll;

        [RootComponent("Doll")]
        public FirstDoll FirstDollDuplicate;
    }
}
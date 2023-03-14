using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

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
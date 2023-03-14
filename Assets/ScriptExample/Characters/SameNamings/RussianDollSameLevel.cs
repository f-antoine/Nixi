using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields.SingleComponent;

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
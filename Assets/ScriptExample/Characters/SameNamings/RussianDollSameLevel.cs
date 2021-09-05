using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevel : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("Doll", "ChildDoll")]
        public FirstDoll Doll;

        [NixInjectMonoBehaviourFromMethodRoot("Doll", "ChildDoll")]
        public FirstDoll SecondDoll;
    }
}
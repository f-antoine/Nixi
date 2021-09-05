using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class RussianDollSameLevelParent : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethodRoot("Doll")]
        public FirstDoll FirstDoll;

        [NixInjectMonoBehaviourFromMethodRoot("Doll")]
        public FirstDoll FirstDollDuplicate;
    }
}
using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class RussianDoll : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public FirstDoll ChildDoll;

        [NixInjectMonoBehaviourFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public FirstDoll ChildDoll2;
    }
}
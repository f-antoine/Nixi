using Nixi.Injections;
using Nixi.Injections.Attributes.ComponentFields;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class RussianDoll : MonoBehaviourInjectable
    {
        [NixInjectComponentFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public FirstDoll ChildDoll;

        [NixInjectComponentFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public FirstDoll ChildDoll2;
    }
}
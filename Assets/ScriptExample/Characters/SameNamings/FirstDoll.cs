using Nixi.Injections;
using Nixi.Injections.Attributes.MonoBehaviours;

namespace Assets.ScriptExample.Characters.SameNamings
{
    public sealed class FirstDoll : MonoBehaviourInjectable
    {
        [NixInjectMonoBehaviourFromMethod("ChildDoll", GameObjectMethod.GetComponentsInChildren)]
        public SecondDoll ChildDoll;
    }
}